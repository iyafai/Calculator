//Imran Yafai
//Comp Phys P6C

#include <iostream>
#include <stdio.h>
#include <math.h>

#define G 0.0000000000667398

using namespace std;

double RK2(double Cap, double Res, double InitV, double end, double step)
{
	double Charge = Cap*InitV, Current = 0, thyme = 0;

//	cout<<"time "<<thyme<<" charge "<<Charge<<endl;		//prints initial value
	do
	{
		Charge+=-1*(step/(Res*Cap))*(Charge-(Charge*step)/(2*Res*Cap));
		thyme+=step;
//		cout<<"time "<<thyme<<" charge "<<Charge<<endl;
	}while(thyme<end);

	return Charge;
}

double RK4(double Cap, double Res, double InitV, double end, double step)
{
	double Charge = Cap*InitV, Current = 0, thyme = 0;
	double k1=0, k2=0, k3=0, k4=0;
	double Q1=0, Q2=0, Q3=0, Q4=0;

//	cout<<"time "<<thyme<<" charge "<<Charge<<endl;		//prints initial value
	do
	{
		k1 = -1*step*Charge/(Res*Cap);
		Q1 = Charge+k1;
		k2 = -1*step*(Charge+k1/2)/(Res*Cap);
		Q2 = Charge+k2;
		k3 = -1*step*(Charge+k2/2)/(Res*Cap);
		Q3 = Charge+k3;
		k4 = -1*step*(Charge+k3)/(Res*Cap);
		Q4 = Charge+k4;
		Charge=(Q1+2*Q2+2*Q3+Q4)/6;
		thyme+=step;
//		cout<<"time "<<thyme<<" charge "<<Charge<<endl;
	}while(thyme<end);

	return Charge;
}

double GravX(double density, double dim, double xpos, double ypos, double step)
{
	double field = 0;

	for(double x=0; x<dim; x+=(dim/step))
		for(double y=0; y<dim; y+=(dim/step))
		{
			field+=-1*density*G*(dim/step)*(dim/step)
			*(x-xpos)/(sqrt((x-xpos)*(x-xpos)+(y-ypos)*(y-ypos))
			*((x-xpos)*(x-xpos)+(y-ypos)*(y-ypos)));
		}
	return field;
}

double GravY(double density, double dim, double xpos, double ypos, double step)
{
	double field = 0;

	for(double x=0; x<dim; x+=(dim/step))
		for(double y=0; y<dim; y+=(dim/step))
		{
			field+=-1*density*G*(dim/step)*(dim/step)
			*(y-ypos)/(sqrt((x-xpos)*(x-xpos)+(y-ypos)*(y-ypos))
			*((x-xpos)*(x-xpos)+(y-ypos)*(y-ypos)));
		}
	return field;
}
int main()
{
	double Cap = 3, Res = 8, InitV = 5, end = .5, step = .001;
	double XVal = 0, YVal =0, Mag = 0;
	double density = 5.0/4.0;

	cout<<"Charge on Capacitor after "<<end<<" seconds"<<endl;
	cout<<"RK2 Method gives: "<<RK2(Cap, Res, InitV, end, step)<<endl;
	cout<<"RK4 Method gives: "<<RK4(Cap, Res, InitV, end, step)<<endl;
	XVal = 1*GravX(density, 2.0, -1.0, 0, 1000);
	YVal = 1*GravY(density, 2.0, -1.0, 0, 1000);
	cout<<"X Component of Grav force = "<<XVal<<" N"<<endl;
	cout<<"Y Component of Grav force = "<<YVal<<" N"<<endl;
	cout<<"Magnitude of Grav Force = "<<sqrt(XVal*XVal+YVal*YVal)<<" N"<<endl;
}