//Imran Yafai
//Comp Phys Project 9

#include <iostream>
#include <stdio.h>
#include "Matrix.h"

using namespace std;

int main()
{
	double input =0;
	Matrix A(2,2);

	cout<<"Input value for (1,1) element of matrix: "<<endl;
	cin>>input;
	A.setEle(0,0,input);

	cout<<"Input value for (1,2) element of matrix: "<<endl;
	cin>>input;
	A.setEle(0,1,input);

	cout<<"Input value for (2,1) element of matrix: "<<endl;
	cin>>input;
	A.setEle(1,0,input);

	cout<<"Input value for (2,2) element of matrix: "<<endl;
	cin>>input;
	A.setEle(1,1,input);

/*	Matrix B(2,2);
	Matrix C(2,2);
	Matrix D(2,2);
	Matrix E(2,2);
	
	for(int i=0; i<2; i++)
	{
		B.setEle(i,i,1);
		for(int j=0; j<2; j++)
			C.setEle(i,j,1);
	}

	A.Print();
	B.Print();
	C.Print();
	D = A.Sub(B);
	E = A.Sub(C);
	D.Print();
	E.Print();*/

	cout<<A.Eigenvalue();
}