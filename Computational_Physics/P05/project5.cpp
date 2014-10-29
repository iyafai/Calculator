//Imran Yafai
//Comp Phys Project 5
#include <iostream>
#include <stdio.h>
#include <math.h>
#include "geometry.h"

#define k 8987551787.368

using namespace std;

double integral(double start, double end)
{
	double valueS = 1/(start*start);
	double valueE = 1/(end*end);
	double sum = 0;
	
	if((valueS-valueE) < .001*valueE)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integral(start, (end+start)/2) + integral((end+start)/2, end);
	return sum;
}

double integral2(double start, double end)
{
	double valueS = 1/(exp(start)*start*start);
	double valueE = 1/(exp(end)*end*end);
	double sum = 0;
	
	if((valueS-valueE) < .000001)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integral2(start, (end+start)/2) + integral2((end+start)/2, end);
	return sum;
}

double integralSub(double start, double end)
{
	double valueS = 1.0/exp(1.0/(1.0-start));
	double valueE = 1.0/exp(1.0/(1.0-end));
	double sum = 0;
	
	if((valueS-valueE) < .0001)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integralSub(start, (end+start)/2) + integralSub((end+start)/2, end);
	return sum;
}

double integralZ(double start, double end)
{
	double valueS = 2/sqrt((4+start*start)*(4+start*start)*(4+start*start));
	double valueE = 2/sqrt((4+end*end)*(4+end*end)*(4+end*end));
	double sum = 0;
	
	if((valueS-valueE) < .001*valueE)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integralZ(start, (end+start)/2) + integralZ((end+start)/2, end);
	return sum;
}

double integralX(double start, double end)
{
	double valueS = start/sqrt((4+start*start)*(4+start*start)*(4+start*start));
	double valueE = end/sqrt((4+end*end)*(4+end*end)*(4+end*end));
	double sum = 0;
	
	if((valueS-valueE) < .001*valueE)
		sum = sqrt((end-start)*(end-start))*(valueS+valueE)/2;
	else
		sum = integralX(start, (end+start)/2) + integralX((end+start)/2, end);
	return sum;
}

int main()
{
	double step1 = 2000.0, step2=500.0, step3=100.0;
	double radius = 1.0;
	double finiteZ = 0, finiteX = 0;
	double infiniteZ =0, infiniteX =0;

	cout<<"area of circle of radius "<<radius<<" = "<<circle(radius, step1)<<endl;
	cout<<"volume of sphere of radius "<<radius<<" = "<<sphere(radius, step2)<<endl;
	cout<<"volume(?) of hyper-sphere of radius "<<radius<<" = "<<hyper(radius, step3)<<endl;

	cout<<"\nMagnitude of electric field at point A: "<<k*3*integral(2,3)<<endl;

	finiteZ = 3*k*integralZ(0,1);
	finiteX = 3*k*integralX(0,1);
	cout<<"\nZ component of electric field at point B: "<<finiteZ<<endl;
	cout<<"X component of electric field at point B: "<<finiteX<<endl;
	cout<<"Magnitude of electric field at point B: "<<sqrt(finiteX*finiteX+finiteZ*finiteZ)<<endl;
	
	cout<<"\nMagnitude of electric field at point A for infinite line charge: "<<k*integral2(1,99)<<endl;
	cout<<"\nMagnitude of electric field at point A for infinite line charge using change of variable method: ";
	cout<<k*integralSub(0,1)<<endl;
}