//Imran Yafai
//Computaional Physics Project 3

#include <iostream>
#include <stdio.h>
#include <math.h>

#define PI 3.1415926535897932384626

using namespace std;

double* LR_integrate(int step, double* x, double* t)
{
	double* temp = new double[step];
	temp[0]=0;

	for(int m=1; m<step; m++)
		temp[m]=(t[m+1]-t[m])*x[m]+temp[m-1];

	return temp;
}

double* MP_integrate(int step, double* x, double* t)
{
	double* temp = new double[step];
	temp[0]=0;

	for(int i=1, k=1; i<(2*step); i+=2, k++)
		temp[k]=(t[i+1]-t[i-1])*x[i]+temp[k-1];

	return temp;
}

int main()
{
	int step=0;					//number of intervals to have for integrating
	double normal=0;				//normalization factor for A*sin(pi*x)
	cout<<"input number of steps to use in integrating 'f(t)=t^3'"<<endl;
	cin>>step;
	
	double* x_data = new double[step+1];		//x for t^3 using LR
	double* x2_data = new double[2*step+1];	//x for t^3 using MP
	double* x3_data = new double[2*step+1];	//x for A*sin(pi*x) using MP

	double* t_data = new double[step+1];		//t for t^3 using LR
	double* t2_data = new double[2*step+1];	//t for t^3 using MP
	double* t3_data = new double[2*step+1];	//t for A*sin(pi*x) using MP

	double* integral = new double;		//integral data of t^3 using LR
	double* integral2 = new double;		//integral data of t^3 using MP
	double* integral3 = new double;		//integral data of A*sin(pi*x) using MP

	for(int i=0; i<=step; i++)
	{
		t_data[i]=(static_cast<double>(i)/step);
		x_data[i]=t_data[i]*t_data[i]*t_data[i];
	}
	integral= LR_integrate(step, x_data, t_data);

	cout<<"Left Rectangle Integral Method:\nt, x(t), integral[x(t)]"<<endl;
	for(int k=0; k<step; k++)
		cout<<t_data[k]<<","<<x_data[k]<<","<<integral[k]<<endl;

	for(int i=0; i<=(2*step); i++)
	{
		t2_data[i]=(static_cast<double>(i)/(2*step));
		x2_data[i]=t2_data[i]*t2_data[i]*t2_data[i];
	}
	integral2 = MP_integrate(step, x2_data, t2_data);
	
	cout<<"\nMid-Point Integral Method:"<<endl;
	for(int k=0; k<=step; k++)
		cout<<t_data[k]<<","<<x_data[k]<<","<<integral2[k]<<endl;

	for(int i=0; i<=(2*step); i++)
	{
		t3_data[i]=(static_cast<double>(i)/(2*step));
		x3_data[i]= sin(PI*t3_data[i]);
	}
	integral3 = MP_integrate(step, x3_data, t3_data);

	cout<<"\nNormalization of A*sin(pi*x) for (0<x<1): "<<endl;
	normal = 1/integral3[step];
	cout<<"A = "<<normal<<endl;	
}
