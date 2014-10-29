//Imran Yafai
//Computaional Physics Project 2B

#include <iostream>
#include <stdio.h>

using namespace std;

int main()
{
	double x_data[21]={0}; 
	double t_data[21]={0};
	double derivative[21]={0};
	double integral[21]={0};
	
	for(int i=0; i<21; i++)
	{
		t_data[i]=.1*i;
		x_data[i]=2*t_data[i]*t_data[i];
	}
	
	derivative[0]=0;
	for(int k=1; k<21; k++)
	{
		derivative[k]=((x_data[k]-x_data[k-1])/(t_data[k]-t_data[k-1]));
	}

	for(int m=1; m<21; m++)
	{
		integral[m]=(t_data[m]-t_data[m-1])*derivative[m]+integral[m-1];
	}
	
	cout<<"t [s],x(t) [m],dx/dt,integral(dx/dt)"<<endl;
	for(int l=0; l<21; l++)
	{
		cout<<t_data[l]<<","<<x_data[l]<<","<<derivative[l]<<","<<integral[l]<<endl;
	}
	
}