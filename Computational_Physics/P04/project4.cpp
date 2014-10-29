//Imran Yafai
//Computational Physics - Project 4
//Small angle approximation value for period = 2*pi*sqrt(length/g) = 1.418503353
//expression derived for period: T = 4*sqrt(length/2g)*integral[1/sqrt[cos(theta)-cos(theta_i)]] from 0 to theta_i

#include <iostream>
#include <stdio.h>
#include <math.h>

#define PI 3.1415926535897932384626

using namespace std;

double integral2(double start, double end)
{
	double valueS = 1/sqrt(start);
	double valueE = 1/sqrt(end);
	double sum = 0;

	if((valueS-valueE) < .001*valueE)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integral2(start, (end+start)/2) + integral2((end+start)/2, end);
	return sum;
}

double integral(double start, double end, double initial)
{
	double valueS = 1/sqrt(cos(start)-cos(initial));
	double valueE = 1/sqrt(cos(end)-cos(initial));
	double sum = 0;
	
	
	if((valueE-valueS) < .001*valueS)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integral(start, (end+start)/2, initial) + integral((end+start)/2, end, initial);
	return sum;
}

int main()
{
	double theta_i = 0.1, length=.5;
	double theta1 = 10*PI/180, theta2 = 20*PI/180, theta3 = 30*PI/180, theta4 = 40*PI/180;
	double value, value1=0, value2=0, value3=0, value4 = 0;
	
	value = 4*sqrt(length/(2*9.81))*integral(0, theta_i-0.0000001, theta_i);
	value1 = 4*sqrt(length/(2*9.81))*integral(0, theta1-0.0000001, theta1);
	value2 = 4*sqrt(length/(2*9.81))*integral(0, theta2-0.0000001, theta2);
	value3 = 4*sqrt(length/(2*9.81))*integral(0, theta3-0.0000001, theta3);
	value4 = 4*sqrt(length/(2*9.81))*integral(0, theta4-0.0000001, theta4);
	
	cout<<"integral of 1/sqrt(x) from 0 to 1 = "<<integral2(0.000000000000000001,1)<<endl;
	cout<<"period of pendulum with initial angle="<<theta_i<<" radians = "<<value<<endl;
	cout<<"period of pendulum with initial angle=10 degrees = "<<value1<<endl;
	cout<<"period of pendulum with initial angle=20 degrees = "<<value2<<endl;
	cout<<"period of pendulum with initial angle=30 degrees = "<<value3<<endl;
	cout<<"period of pendulum with initial angle=40 degrees = "<<value4<<endl;
}