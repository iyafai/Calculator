//Imran Yafai
//Computational Physics Project6E
#include <iostream>
#include <stdio.h>

using namespace std;

#define G 0.0000000000667398
#define SunMass 1.9891E30
#define CometMass 220000000000000000.0
#define Perihelion 87664352200.0

double Period(double vix, double viy, double InitX, double InitY, double thymestep)
{
	double vx = vix, vy = viy;
	double x = InitX, y = InitY;
	double v = sqrt(vx*vx+vy*vy), r=sqrt(x*x+y*y), thyme=0;
	double ax = G*SunMass*x/(r*r*r), ay = G*SunMass*y/(r*r*r);
	
	do
	{
		x+=vx*thymestep;
		y+=vy*thymestep;
		vx+=-ax*thymestep;
		vy+=-ay*thymestep;
		v = sqrt(vx*vx+vy*vy);
		r = sqrt(x*x+y*y);
		ax = G*SunMass*x/(r*r*r);
		ay = G*SunMass*y/(r*r*r);
		thyme+=thymestep;
	//	cout<<thyme<<" x "<<x<<" y "<<y<<" vx "<<vx<<" vy "<<vy<<" ax "<<ax<<" ay "<<ay<<endl;
	}while(y > 0);
	
	return (2*thyme);
}

int main()
{
	double period = 0;
	period = Period(0,54000.0, Perihelion, 0, 3600);
	cout<<"Period of Halley's Comet Orbit = "<<(period/(60*60*24*365.25))<<" years"<<endl;
}