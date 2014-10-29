//Imran Yafai
//Computational Physics Project 1
//Purpose: Running algorithims for finding the square root of a number

#include <stdio.h>
#include <iostream>
#include "sqrt_algorithm.h"


using namespace std;

int main()
{
	double input, output1=0, output2=0, output3=0;
	cout<<"Input number to be square rooted: "; //outputs text, called from iostream
	cin>>input;				    //takes input from user and stores, called from iostream
	output1= sqrt_brute(input);  //square root algorithm called from sqrt_algorithm.h
	output2= sqrt_guess(input);  //same as above
	output3= sqrt_newton(input); //same as above

	cout<<"Result of bruteforce attempt: "<<output1<<endl;
	cout<<"Result of guessing attempt: "<<output2<<endl;
	cout<<"Result of Newton's Method: "<<output3<<endl;
	
	return 0;
}