//Imran Yafai
//Computaional Physics Homework 4

#include <iostream>
#include <stdio.h>

using namespace std;

double* LR_integrate(int step, double* x, double* t)
{
	double* temp = new double[step];
	temp[0]=0;

	for(int m=1; m<step; m++)
		temp[m]=(t[m+1]-t[m])*x[m]+temp[m-1];

	return temp;
}

int main()
{
	/*
	int step=0;		//number of intervals to have for integrating
	cout<<"input number of steps to use in integrating 'f(t)=5t^3'"<<endl;
	cin>>step;*/
	double sumLR = 0;
	double sumMP = 0;
	double sumTP = 0;
/*
	double* x_data = new double[step+1];	//x for 5t^3 using LR
	double* t_data = new double[step+1];	//t for 5t^3 using LR
	
	double* integral = new double;		//integral data of 5t^3 using LR

	for(int i=0; i<=step; i++)
	{
		t_data[i]=(static_cast<double>(i)/step);
		x_data[i]=5*t_data[i]*t_data[i]*t_data[i];
	}
	integral= LR_integrate(step, x_data, t_data);
	cout<<"t, x(t), integral[x(t)]"<<endl;
	for(int k=0; k<step; k++)
		cout<<t_data[k]<<","<<x_data[k]<<","<<integral[k]<<endl;
	cout<<"Error,"<<step<<"steps,"<<1.25-integral[step-1]<<endl;
	*/
	cout<<"N,Error_LR, Error_MP, Error_TP"<<endl;
	for(int l=5; l<=100; l++)
	{
		for(int p=1; p<l; p++)
		{
			sumLR+=((1/static_cast<double>(l))*5
			*static_cast<double>(p)/static_cast<double>(l)
			*static_cast<double>(p)/static_cast<double>(l)
			*static_cast<double>(p)/static_cast<double>(l));
		}
		for(int r=0; r<l; r++)
		{
			sumMP+=((1/static_cast<double>(l))*5
			*static_cast<double>(r+.5)/static_cast<double>(l)
			*static_cast<double>(r+.5)/static_cast<double>(l)						*static_cast<double>(r+.5)/static_cast<double>(l));
		}
		for(int n=0; n<l; n++)
		{
			sumTP+=((1/static_cast<double>(l))*5*
			(static_cast<double>(n)/static_cast<double>(l)
			*static_cast<double>(n)/static_cast<double>(l)						*static_cast<double>(n)/static_cast<double>(l)
			+static_cast<double>(n+1)/static_cast<double>(l)
			*static_cast<double>(n+1)/static_cast<double>(l)						*static_cast<double>(n+1)/static_cast<double>(l))/2);
		}
		cout<<l<<","<<(1.25-sumLR)<<","<<(1.25-sumMP)<<","<<(sumTP-1.25)<<endl;
		sumLR=0;
		sumMP=0;
		sumTP=0;
	}
	
}