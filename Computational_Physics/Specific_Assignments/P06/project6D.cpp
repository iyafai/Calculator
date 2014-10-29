//Imran Yafai
//Comp Phys P6D

#include <iostream>
#include <stdio.h>
#include <math.h>
#include <cmath>

#define PI atan(1)*4

using namespace std;

double ProjNoDrag(double angle, double v_i, double step)
{
	double v_x =v_i*cos(angle*PI/180);
	double v_y =v_i*sin(angle*PI/180);
	double xpos = 0, ypos = 0, a = -9.8;
	double thyme = 0;
	double thymestep = 1/step;

	do
	{
		xpos+=v_x*thymestep;
		ypos+=v_y*thymestep;
		v_y+=a*thymestep;
		thyme+=thymestep;
	}
	while(ypos>0);

	return xpos;
}

double ProjDrag(double mass, double angle, double v_i, double drag, double thymestep)
{
	double v_x =v_i*cos(angle*PI/180);
	double v_y =v_i*sin(angle*PI/180);
	double v = v_i;
	double xpos = 0, ypos = 0, a_x=0, a_y = -9.8, a = 0;
	double thyme = 0;

//	cout<<"x,y"<<endl;
//	cout<<xpos<<","<<ypos<<endl;

	do
	{
		v = sqrt(v_x*v_x+v_y*v_y);
		a_x=-drag*v*v*sqrt(v_x*v_x)/(v*mass);
		a_y=-9.8-drag*v*v_y/(mass);
		v_y+=a_y*thymestep;
		v_x+=a_x*thymestep;
		xpos+=v_x*thymestep;
		ypos+=v_y*thymestep;
//		cout<<xpos<<","<<ypos<<endl;

		thyme+=thymestep;
	}while(ypos>0);
	

	return xpos;
}

double vel(double distance, double mass, double angle, double guess, double drag, double thymestep)
{
	double guessD = 0;
	guessD = ProjDrag(mass, angle, guess, drag, thymestep);
	if(abs(guessD - distance) > .001*distance)
	{
		if(guessD>distance)
			guess-=.001*guess;
		else
			guess+=.001*guess;
		guess = vel(distance, mass, angle, guess, drag, thymestep);
	}
	else
		return guess;
}

int main()
{
	double guess1=0;
	double guess2=0;
	cout<<"Distance w/o air drag = "<<ProjNoDrag(30, 80, 1000)<<endl;
	cout<<"Distance w/air drag (.001) = "<<ProjDrag(.15, 30, 80,.001, .00001)<<endl;
	cout<<"Input Guess for velocity needed for ball to go 100 m in New York (coefficient=.001)"<<endl;
	cin>>guess1;
	cout<<"Calculating... (This may take a while)"<<endl;
	cout<<"velocity = "<<vel(100, .15, 30, guess1,.001, .0001)<<endl;

	cout<<"\nInput Guess for velocity needed for ball to go 100 m in Denver (coefficient=.0008)"<<endl;
	cin>>guess2;
	cout<<"Calculating... (This may take a while)"<<endl;
	cout<<"velocity = "<<vel(100, .15, 30, guess2,.0008, .0001)<<endl;
}