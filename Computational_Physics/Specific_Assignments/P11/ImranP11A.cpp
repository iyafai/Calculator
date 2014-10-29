#include <iostream>
#include <stdio.h>
#include <cstdlib>
#include <ctime>
#include <math.h>
#define rate .05776226504666

using namespace std;

int main()
{
	srand(time(0));
	int head =0, tail =0, r=0;
	int atoms = 1000; 
	double above=0, temp=0, temp2=0;

	for(int i=0; i<1000; i++)
	{
		temp = (double)rand()/(double)RAND_MAX;
		if(temp<.5)
			head++;
		else
			tail++;
	}

	cout<<"number of heads: "<<head<<" number of tails: "<<tail<<endl;

	for(double j=0; j<(1000); j++)
	{
		temp2 = (double)rand()/(double)RAND_MAX;

		if(temp2<rate)
			atoms--;
	}

	cout<<"predicted number: "<<exp(-0.057762265)*1000<<endl;

	cout<<atoms<<" atoms left after day one"<<endl;

	for(int k=0; k<100000; k++)
	{
		for(double j=0; j<(1000); j++)
		{
			temp2 = (double)rand()/(double)RAND_MAX;
	
			if(temp2<rate)
				atoms--;
		}

		if(atoms>940)
			above++;
		atoms = 1000;
	}
	cout<<"percentage above 940 atoms: "<<above/1000.0<<"%"<<endl;
}