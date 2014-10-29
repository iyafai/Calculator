//Imran Yafai
//Computational Physics - Project 8A

#include <iostream>
#include <stdio.h>
#include "Vector.h"

#define e 1.6022E-19

using namespace std;

int main()
{
	Vector v(2,5,9);
	Vector B(2,7,1);
	cout<<"v = ";
	v.Print();
	cout<<"\nB = ";
	B.Print();
	cout<<"\nDot Product of V and B = "<<v.Dot(B)<<endl;
	Vector VxB= v.Cross(B);
	cout<<"v x B = ";
	VxB.Print();
	cout<<"\nMagnetic Force on electron = ";
	Vector Force = VxB.Scalar(e);
	Force.Print();
	cout<<" N"<<endl;
}