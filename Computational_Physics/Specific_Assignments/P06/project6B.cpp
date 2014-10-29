//Imran Yafai
//Computational Physics - Project 6B
#include <iostream>
#include <stdio.h>
#include <math.h>

using namespace std;

double IVP(double Cap, double Res, double InitV, double end, double step)
{
	double Charge = Cap*InitV, Current = 0, thyme = 0;

	cout<<"time "<<thyme<<" charge "<<Charge<<endl;		//prints initial value
	do
	{
		Current = -1*Charge/(Res*Cap);
		Charge+=Current*step;
		thyme+=step;
		cout<<"time "<<thyme<<" charge "<<Charge<<endl;
	}while(thyme<end);

	return Charge;
}

int main()
{
	double Cap = 3, InitV = 5, Res = 8, end = .5, step = .25;
	cout<<"Charge on capacitor after .5 seconds: "<<IVP(Cap, Res, InitV, end, step)<<endl;
}