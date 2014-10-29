//Imran Yafai
//Computational Physics Project 6F
#include <iostream>
#include <stdio.h>
#include <math.h>

#define PI atan(1)*4
#define k1 9.8
#define k2 11.667
#define mass .25

using namespace std;

void CoupOsc(double x1i, double x2i, double angle, double thymestep, double end)
{
	double x1 = x1i, x2 = x2i, v1 = 0, v2 = 0;
	double a1 = 0, a2 = 0, thyme = 0;

	do
	{
		cout<<thyme<<","<<x2<<endl;
		a1 = k2*(x2-x1)/mass-k1*x1/mass;
		a2 = -k2*(x2-x1)/mass;
		v1+=a1*thymestep;
		v2+=a2*thymestep;
		x1+=v1*thymestep;
		x2+=v2*thymestep;
		thyme+=thymestep;
	}while(thyme < end);
}

void CoupOscFric(double x1i, double x2i, double thymestep, double end)
{
	double x1 = x1i, x2 = x2i, v1 = 0, v2 = 0;
	double a1 = 0, a2 = 0, thyme = 0;

	do
	{
		cout<<thyme<<","<<x2<<endl;
		a1 = k2*(x2-x1)/mass-k1*x1/mass - .7*v1;
		a2 = -k2*(x2-x1)/mass - .7*v2;
		v1+=a1*thymestep;
		v2+=a2*thymestep;
		x1+=v1*thymestep;
		x2+=v2*thymestep;
		thyme+=thymestep;
	}while(thyme < end);
}

int main()
{
	double angle = 7.5*PI/180;
	double x1 = .005, x2 = 0;
	CoupOscFric(x1, x2, .01, 20.0);
}