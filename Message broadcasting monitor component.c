#include <stdio.h>
#include <stdlib.h>
#include <string.h>

// Maximal sizes of system
#define MAX_CHAR 100
#define MAX_ANOMALILES_SIZE 1000
// ECU components converted from Hex. numbers
#define ECU_SPEEDOMETER 0x100
#define ECU_PEDALS 0x200
#define ECU_ABS 0x400
#define ECU_TIRE_PRESSURE 0x800
// Frequencies of message transmissions
#define SPEEDOMETER_MSG_FREQUENCY 50
#define PEDALS_MSG_FREQUENCY 5
#define ABS_MSG_FREQUENCY 10
#define TIRE_MSG_FREQUENCY 100
// Values for ECU components
#define MAX_CURRENT_SPEED_VALUE 300
#define MAX_ACCELERATION_VALUE 100
#define MAX_BRAKE_VALUE 100
#define MAX_ABS_VALUE 1
#define MAX_PRESSURE_PERCENTAGE 100
#define MAX_SPEED_CHANGE 5
#define MIN_PEDALS_PRESS_TIME 10
#define HARD_PRESS_ON_BRAKES 80
#define MIN_TIRE_PRESSURE 30
#define MAX_SPEED_OF_LOW_TIRE_PRESSURE 50

// Defining the system's components according to SBP protocol

//Message object
typedef struct{
	unsigned int messageID;
	unsigned int timeStamp;
	unsigned int ecuID;
	unsigned int firstVal;
	unsigned int secondVal;
} Message_st;
typedef Message_st* Message;

//Speedometer ECU component
typedef struct{
	unsigned int lastTimeChanged;
	unsigned int currentVal;
	unsigned int driveCrashed;
} Speedometer_st;
typedef Speedometer_st* Speedometer;

//Pedals ECU component
typedef struct{
	unsigned int lastTimeChanged;
	unsigned int accelerationVal;
	unsigned int brakeVal;
	unsigned int lastAcceleratorChange;
	unsigned int lastBrakeChange;
} Pedals_st;
typedef Pedals_st* Pedals;

//ABS ECU component
typedef struct{
	unsigned int lastTimeChanged;
	unsigned int active;
} ABS_st;
typedef ABS_st* ABS;

//Tire ECU component
typedef struct{
	unsigned int lastTimeChanged;
	unsigned int percentage;
} Tire_st;
typedef Tire_st* Tire;


int resetComponents(Speedometer* speedometer, Pedals* pedals, ABS* abs, Tire* tire){

	/* Allocate memory for ECU components and reset their fields
	 *
	 * Speedometer ECU memory allocation, check and value reset
	 */
	*speedometer = (Speedometer) malloc (sizeof(Speedometer_st));
	if(!speedometer){
		perror("No memory available!\n");
		return -1;
	}
	(*speedometer)->currentVal = 0;
	(*speedometer)->lastTimeChanged = 0;
	(*speedometer)->driveCrashed = 0;

	// Pedals ECU memory allocation, check and value reset
	*pedals = (Pedals) malloc (sizeof(Pedals_st));
	if(!pedals){
		perror("No memory available!\n");
		return -1;
	}
	(*pedals)->accelerationVal= 0;
	(*pedals)->brakeVal = 0 ;
	(*pedals)->lastTimeChanged = 0;
	(*pedals)->lastAcceleratorChange = 0;
	(*pedals)->lastBrakeChange = 0;

	// ABS ECU memory allocation, check and value reset
	*abs = (ABS) malloc (sizeof(ABS_st));
	if(!abs){
		perror("No memory available!\n");
		return -1;
	}
	(*abs)->active = 0;
	(*abs)->lastTimeChanged = 0;

	// Tire ECU memory allocation, check and value reset
	*tire = (Tire) malloc (sizeof(Tire_st));
	if(!tire){
		perror("No memory available!\n");
		return -1;
	}
	(*tire)->percentage = 100;
	(*tire)->lastTimeChanged = 0;

	return 0;
}

int createMessageFromLine(Message msg, char *lineMessage){

	/* Create a Message struct from messages written as lines:
	 * Go over the parts of the message as strings.
	 * Each part of message braked down into separate string.
	 * Convert relevant field from string to Uinr32 relevant in Message_st.
	 */
	char* msgField = strtok (lineMessage, " ");
	char allFields[5][20];
	int indexOfField = 0;
	// Iterate over message parts and save them separately
	while(msgField != NULL){
		strcpy(allFields[indexOfField], msgField);
		msgField = strtok (NULL, " \n\0");
		indexOfField++;
	}
	// Set field from string to relevant Message's field as Uint32
	msg->messageID = strtoul(allFields[0], NULL, 10);
	msg->timeStamp = strtoul(allFields[1], NULL, 10);
	msg->ecuID = strtoul(allFields[2], NULL, 16);
	msg->firstVal = strtoul(allFields[3], NULL, 10);
	msg->secondVal = strtoul(allFields[4], NULL, 10);

	return 0;
}

int CarSpeedAnomaly(Message* msg,  Speedometer* speedometer){
	unsigned int isAnomaly;
	int changeInSpeed = abs((*msg)->secondVal - (*speedometer)->currentVal);
	if((*speedometer)->driveCrashed == 1 && (*msg)->secondVal != 0){
		isAnomaly = 1;
	} else // Check whether message got within frequency range
		if( (*msg)->timeStamp - (*speedometer)->lastTimeChanged > SPEEDOMETER_MSG_FREQUENCY){
		isAnomaly = 0;
	} else if(changeInSpeed > MAX_SPEED_CHANGE){ // Message got delivered within range of SPEEDOMETER_MSG_FREQUENCY. Need to check speed change value.
		if( (*msg)->secondVal == 0){ // Exception of car crash
			isAnomaly = 0;
			(*speedometer)->driveCrashed = 1;
		}
		else { // Speed changed more than it should
			isAnomaly = 1;
		}
	}
	return isAnomaly;
}

int pedalsIsPressedUnderMinimum(Message* msg, Pedals* pedals){
	unsigned int wrongPress = 0;
	unsigned int changeInAcceleratorTime = (*msg)->timeStamp - (*pedals)->lastAcceleratorChange;
	unsigned int changeInBrakesTime = (*msg)->timeStamp - (*pedals)->lastBrakeChange;
	// Current pedal isn't pressed and MIN_PEDALS_PRESS before that time was pressed
	if( (*msg)->firstVal == 0 && (*pedals)->accelerationVal != 0 && (changeInAcceleratorTime < MIN_PEDALS_PRESS_TIME) ){
		wrongPress = 1;
	} else if( (*msg)->secondVal == 0 && (*pedals)->brakeVal != 00 && (changeInBrakesTime < MIN_PEDALS_PRESS_TIME) ){
		wrongPress = 1;
	}
	return wrongPress;

}


int detect_timing_anomalies(const char* file_path, unsigned int *anomalies_ids)
{
	unsigned int indexOfAnomali = 0;

	// Allocate new file descriptor
	FILE* sourceFile = fopen(file_path, "r");
	// Check if memory allocation is valid
	if(!sourceFile){
		perror("Problem with file open!\n");
		return -1;
	}

	// Build a new system and reset it
	Speedometer speedometer = NULL;
	Pedals pedals = NULL;
	ABS abs = NULL;
	Tire tire = NULL;

	resetComponents(&speedometer, &pedals, &abs, &tire);

	// Allocate memory for message object to be used
	Message msg = malloc (sizeof(Message_st));
	// Check if memory allocation is valid
	if(!msg){
		perror("No memory available!\n");
		return -1;
	}

	// Initialize array with MAX_CHAR size for line reading
	char line[MAX_CHAR];
	// Iterating over the messages and detect first MAX_ANOMALOUS_SIZE anomalies
	while( (fgets(line, MAX_CHAR, sourceFile) != NULL) && (indexOfAnomali < MAX_ANOMALILES_SIZE) ){
		createMessageFromLine(msg, line);
		// Detect abnormal frequency by ECU id
		switch(msg->ecuID){
			case ECU_SPEEDOMETER:
				// Check if message is within the frequency allowed
				if( (speedometer->lastTimeChanged != 0) && (msg->timeStamp - speedometer->lastTimeChanged < SPEEDOMETER_MSG_FREQUENCY) ){
					*(anomalies_ids + indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				speedometer->lastTimeChanged = msg->timeStamp;
				break;

			case ECU_PEDALS:
				// Check if message is within the frequency allowed
				if( (pedals->lastTimeChanged != 0) && (msg->timeStamp - pedals->lastTimeChanged < PEDALS_MSG_FREQUENCY) ){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				pedals->lastTimeChanged = msg->timeStamp;
				break;

			case ECU_ABS:
				// Check if message is within the frequency allowed
				if( (abs->lastTimeChanged != 0) && (msg->timeStamp - abs->lastTimeChanged < ABS_MSG_FREQUENCY) ){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				abs->lastTimeChanged = msg->timeStamp;
				break;

			case ECU_TIRE_PRESSURE:
				// Check if message is within the frequency allowed
				if( (tire->lastTimeChanged != 0) && (msg->timeStamp - tire->lastTimeChanged < TIRE_MSG_FREQUENCY) ){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				tire->lastTimeChanged = msg->timeStamp;
				break;
		}
	}
	// Free all allocated memory and close stream
	free(msg);
	free(speedometer);
	free(pedals);
	free(abs);
	free(tire);
	fclose(sourceFile);

	return 0;
}



int detect_behavioral_anomalies(const char* file_path, unsigned int *anomalies_ids)
{
	unsigned int indexOfAnomali = 0;

	// Allocate new file descriptor
	FILE* sourceFile = fopen(file_path, "r");
	// Check if memory allocation is valid
	if(!sourceFile){
		perror("Problem with file open!\n");
		return -1;
	}

	// Build a new system and reset it
	Speedometer speedometer = NULL;
	Pedals pedals = NULL;
	ABS abs = NULL;
	Tire tire = NULL;

	resetComponents(&speedometer, &pedals, &abs, &tire);

	// Allocate memory for message object to be used
	Message msg = malloc (sizeof(Message_st));
	// Check if memory allocation is valid
	if(!msg){
		perror("No memory available!\n");
		return -1;
	}

	// Initialize array with MAX_CHAR size for line reading
	char line[MAX_CHAR];
	// Iterating over the messages and detect first MAX_ANOMALOUS_SIZE anomalies
	while( (fgets(line, MAX_CHAR, sourceFile) != NULL) && (indexOfAnomali < MAX_ANOMALILES_SIZE) ){
		createMessageFromLine(msg, line);
		// Detect behavioral anomalies by ECU id
		switch(msg->ecuID){
			case ECU_SPEEDOMETER:
				// Check if values are within the range
				if(msg->secondVal > MAX_CURRENT_SPEED_VALUE){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}else if(CarSpeedAnomaly(&msg, &speedometer) == 1){ // Check speed Anomaly
						*(anomalies_ids+indexOfAnomali) = msg->messageID;
						indexOfAnomali++;
				}
				speedometer->currentVal = msg->secondVal;
				speedometer->lastTimeChanged = msg->timeStamp;
				break;

			case ECU_PEDALS:
				// Check if values are within the range
				if (msg->firstVal > MAX_ACCELERATION_VALUE || msg->secondVal > MAX_BRAKE_VALUE){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				} else if (msg->firstVal != 0 && msg->secondVal !=0){ // If Accelerator and Brakes pressed together
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				} else if ( pedalsIsPressedUnderMinimum(&msg, &pedals)){ // In this case at least one of Accelerator or Brakes is 0
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				// Accelerator is now pressed -> update last pedals change time
				if( pedals->accelerationVal == 0 && msg->firstVal != 0){
					pedals->lastAcceleratorChange = msg->timeStamp;
				}
				// Brakes are now pressed -> update last pedals change time
				if( pedals->brakeVal == 0 && msg->secondVal != 0){
					pedals->lastBrakeChange = msg->timeStamp;
				}
				pedals->accelerationVal = msg->firstVal;
				pedals->brakeVal = msg->secondVal;
				break;

			case ECU_ABS:
				// Check if values are within the range
				if (msg->secondVal > MAX_ABS_VALUE){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				break;
				// No need to update values

			case ECU_TIRE_PRESSURE:
				// Check if values are within the range
				if (msg->secondVal > MAX_PRESSURE_PERCENTAGE){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				break;
				// No need to update values

		}

	}
	// Free all allocated memory and close stream
	free(msg);
	free(speedometer);
	free(pedals);
	free(abs);
	free(tire);
	fclose(sourceFile);

	return 0;
}



int detect_correlation_anomalies(const char* file_path, unsigned int *anomalies_ids)
{
	unsigned int indexOfAnomali = 0;

	// Allocate new file descriptor
	FILE* sourceFile = fopen(file_path, "r");
	// Check if memory allocation is valid
	if(!sourceFile){
		perror("Problem with file open!\n");
		return -1;
	}

	// Build a new system and reset it
	Speedometer speedometer = NULL;
	Pedals pedals = NULL;
	ABS abs = NULL;
	Tire tire = NULL;

	resetComponents(&speedometer, &pedals, &abs, &tire);

	// Allocate memory for message object to be used
	Message msg = malloc (sizeof(Message_st));
	// Check if memory allocation is valid
	if(!msg){
		perror("No memory available!\n");
		return -1;
	}

	// Initialize array with MAX_CHAR size for line reading
	char line[MAX_CHAR];
	// Iterating over the messages and detect first MAX_ANOMALOUS_SIZE anomalies
	while( (fgets(line, MAX_CHAR, sourceFile) != NULL) && (indexOfAnomali < MAX_ANOMALILES_SIZE) ){
		createMessageFromLine(msg, line);
		switch(msg->ecuID){
		// Detect correlations anomalies ECU id
			case ECU_SPEEDOMETER:
				// Check if speed not decreasing when car is accelerating
				if( (msg->secondVal < speedometer->currentVal) && pedals->accelerationVal > 0){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				} else if( (msg->secondVal > speedometer->currentVal) && pedals->brakeVal > 0){ // Check if speed not increasing when car is braking
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				} else if ( msg->secondVal > MAX_SPEED_OF_LOW_TIRE_PRESSURE && tire->percentage < MIN_TIRE_PRESSURE ){ // Check if car is driving under risk
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				speedometer->currentVal = msg->secondVal;
				break;

			case ECU_PEDALS:
				// Hard press on brakes
				if(msg->secondVal > HARD_PRESS_ON_BRAKES){
					abs->active = 1;
				} else{ // Brakes aren't pressed hard anymore
					abs->active = 0;
				}
				pedals->accelerationVal = msg->firstVal;
				pedals->brakeVal = msg->secondVal;
				break;

			case ECU_ABS:
				// Check if ABS is active while hard pressing on brakes
				if(msg->secondVal == 0 && (pedals->brakeVal > HARD_PRESS_ON_BRAKES) ){
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				abs->active = msg->secondVal;
				break;

			case ECU_TIRE_PRESSURE:
				// Check if car is driving under risk
				if (msg->secondVal < MIN_TIRE_PRESSURE && speedometer->currentVal > MAX_SPEED_OF_LOW_TIRE_PRESSURE){ 
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				} else if (msg->secondVal > tire->percentage && speedometer->currentVal != 0){
					// Tires change PSI when car is moving
					*(anomalies_ids+indexOfAnomali) = msg->messageID;
					indexOfAnomali++;
				}
				tire->percentage = msg->secondVal;
				break;
		}
	}
		// Free all allocated memory and close stream
		free(msg);
		free(speedometer);
		free(pedals);
		free(abs);
		free(tire);
		fclose(sourceFile);

		return 0;
}




int main() {
	unsigned int arr[1000];
	for(int i=0; i< 1000; i++){
		arr[i] = 0;
	}
	detect_correlation_anomalies("/Users/afikshani/Downloads/Argus_C_junior_exercise/example_inputs/correlation/22195_26468.txt" , arr);

	for(int j=0; j< 1000; j++){
		if(arr[j] != 0)
		printf("%d\n", arr[j]);
	}

	return 0;

}


