//Imran Yafai
//Computational Physics - Project 6A
#include <iostream>
#include <stdio.h>
#include <math.h>

#define g 9.8
using namespace std;

double IVP(double x_i, double v_i, double a_i, double step)
{
	double x=x_i, v=v_i, a=a_i, thyme=0, thyme_step=1/step;

	do
	{
		v+=a*thyme_step;
		x+=v*thyme_step;
		thyme+=thyme_step;
	}
	while(x>=0);

	return thyme;
}

double IVP2(double x_i, double v_i, double a_i, double step)
{
	double x=x_i, v=v_i, a=a_i, thyme=0, thyme_step=1/step;

	do
	{
		a=a_i+2*sqrt(v*v);
		v+=a*thyme_step;
		x+=v*thyme_step;
		thyme+=thyme_step;
	}
	while(x>=0);

	return thyme;
}

double IVP3(double x_i, double v_i, double a_i, double step)
{
	double x=x_i, v=v_i, a=a_i, thyme=0, thyme_step=1/step;

	do
	{
		a=a_i+2*v*v;
		v+=a*thyme_step;
		x+=v*thyme_step;
		thyme+=thyme_step;
	}
	while(x>=0);

	return thyme;
}

double IVP4(double x_i, double v_i, double a_i, double step)
{
	double x=x_i, v=v_i, a=a_i, thyme=0, thyme_step=1/step;
	double v_max=v_i;

//	cout<<"t,v(t)"<<endl;
	do
	{	
		if(sqrt(v*v)>sqrt(v_max*v_max))
			v_max=v;
//		cout<<thyme<<","<<v<<endl;
		a=a_i+.1*1.2*exp(-.0001*x)*v*v/80;	//assume average weight = 80 kg
		v+=a*thyme_step;
		x+=v*thyme_step;
		thyme+=thyme_step;
	}
	while(x>=0);
	cout<<"max_velocity,"<<v_max<<endl;

	return thyme;
}

int main()
{
	cout<<"time for particle falling from one meter w/o drag "<<IVP(1.0, 0, -g, 3500.0)<<" s"<<endl;
	cout<<"time for particle falling from one meter w/linear drag "<<IVP2(1.0, 0, -g, 3500.0)<<" s"<<endl;
	cout<<"time for particle falling from one meter w/quadratic drag "<<IVP3(1.0, 0, -g, 3500.0)<<" s"<<endl;
	cout<<"time for Baumgartner to fall to earth "<<IVP4(40000, 0, -9.67, 50)<<" s"<<endl;
//	cout<<IVP4(40000, 0, -9.67, 50)<<endl;
}