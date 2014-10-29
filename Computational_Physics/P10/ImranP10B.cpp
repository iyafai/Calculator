//Imran Yafai
//Comp Phys Project 10b

#include <iostream>
#include <stdio.h>
#include "Matrix.h"
#define X 20.0
#define Y 20.0

using namespace std;

double** LaplaceEq2D (double** Data, int Xpts, int Ypts)
{
	double** tmp = new double*[Xpts];
	for(int k=0; k<Ypts; k++)
		tmp[k] = new double[Ypts];
	
	tmp = Data;
	int iter = 0;

	while(iter < 10000)
	{
		for(int i=1; i<19; i++)
			for(int j=1; j<19; j++)
			{
				Data[i][j]= (Data[i-1][j]+Data[i][j-1]+Data[i][j+1]+Data[i+1][j])/4.0;
					
			}
		iter++;
	}

	return tmp;
}

int main()
{
	Matrix Vdata(20,20);
	double** data = new double*[X];
	int x, y, repeat=1;
	double Ex, Ey;

	for(int k=0; k<Y; k++)
		data[k] = new double[Y];

	for(int i=0; i<20.0; i++)
		for(int j=0; j<20; j++)
			Vdata.setEle(i,j,0);

	for(int l=1; l<19; l++)
	{
		data[l][19] = 5.0;
		Vdata.setEle(l,19,5.0);
	}

	data = LaplaceEq2D(data, 20,20);

	for(int m=0; m<Y; m++)
	{
		for(int n=0; n<Y; n++)
			cout<<data[m][n]<<",";
		cout<<endl;
	}

	while(repeat != 0)
	{
		cout<<"Choose Coordinate pair to output the Electric Field and Voltage for: "<<endl;
		cout<<"x: ";		cin>>x;
		cout<<"y: ";		cin>>y;
		
		Ex = -1*(data[x-1][y]-data[x+1][y])/(2.0*2.0/X);
		Ey = -1*(data[x][y-1]-data[x][y+1])/(2.0*2.0/Y);
		
		cout<<"The E field is: " <<Ex<<" i + "<<Ey<<" j"<< endl;
		cout<<"The Voltage at that point is: "<<data[x][y]<<endl;
		cout<<"Choose another pair? (1 for yes, 0 for no)"<< endl;
		cin>>repeat;
	}
/*	do		//failed attempt
	{
		!redo;
		cout<<"input coordinates to check V and E values (0-19) x: ";
		cin>>x;
		cout<<" y: ";
		cin>>y;
		cout<<data[x][y]<<endl;

		cout<<"Repeat? y/n"<<endl
		cin>>rp;
		if(rp =='y')
			redo=='true';
	}
	while(redo);*/
}