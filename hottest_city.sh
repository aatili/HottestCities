#!/bin/bash
#Candidate Task - Adnan Atili
#enivormint variable containting path to binary application
export GET_CURRENT_WEATHER_BIN="./GetCurrentWeather/bin/Debug/net6.0"
#moving to the application folder
cd "$GET_CURRENT_WEATHER_BIN"

arrReturnValue=()
arrRes=()
i=0

while IFS=" /t/n" read -r line; do #reading from given file
	if [[ -z "${line// }" ]]		# skip if line is empty 
	then
		continue
	fi
	(./GetCurrentWeather --city "$line" 
	arrReturnValue[$i]=$?
	if [ "${arrReturnValue[i]}" != "0" ] #city not found
	then
		echo "Could not pull the weather info for $line." 
	fi) &
	((i+=1)) 
	
done > temp.txt #due to parallel application calls , output to a temporary file
wait

arrOutput=() #array contains output format example: Tel-Aviv (25.44C)
arrTem=()	 #array contains temperatures (keeping in order with array above)
while IFS=" /t/n" read -r line; do
	if [ "${line:0:9}" == "Could not" ] #printing error message for cities not found
	then
		echo "$line" 
	else
		temp1=$(echo $line | cut -d "|" -f 1)	#extracting city from A|B|C|D|E format text
		temp2=$(echo $line | cut -d "|" -f 2)	#extracting temperature
		arrOutput+=("${temp1} (${temp2}C)")
		arrTem+=($temp2)
	fi
done < temp.txt

first_max=-99.99
second_max=-99.99
third_max=-99.99
first_output=" "
second_output=" " 
third_output=" "

i=0
for value in "${arrTem[@]}"
do
	temp1=$(echo $value | cut -d "." -f 2) #checking value has two decimals
	temp2_len=`expr length "$temp1"`
	if [[ $temp2_len == 1 ]] #adding a decimal if one is missing
	then
		value+="0"		
	fi
	#finding max 3 temperatures
	#comparison is made after multiplying by 100 since bash does not support floats
	if [[ $((100*${value/.})) -gt $((100*${first_max/.})) ]] 
	then
        third_max=$second_max
		third_output=$second_output
        second_max=$first_max
		second_output=$first_output
        first_max=$value
		first_output=${arrOutput[i]}
	elif [[ $((100*${value/.})) -gt $((100*${second_max/.})) ]]
	then
		third_max=$second_max
		third_output=$second_output
		second_max=$value
		second_output=${arrOutput[i]}
	elif [[ $((100*${value/.})) -gt $((100*${third_max/.})) ]]
	then
		third_max=$value
		third_output=${arrOutput[i]}
    fi
	((i+=1)) 
done

echo "Three hottest cities are:"
echo "1.${first_output}"
echo "2.${second_output}"
echo "3.${third_output}"




rm temp.txt	#removing temporary file