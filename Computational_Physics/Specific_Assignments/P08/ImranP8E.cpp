//Imran Yafai
//Comp Phys P8E

//Eigenvalue 1 - Eigenvector [1 5]	- corresponds to what was seen in program output of 16.6667:83.3333
//Eigenvalue .4 - Eigenvector [-1 1]

#include <iostream>
#include <stdio.h>
#include "Matrix.h"

using namespace std;

int main()
{
	Matrix ELevel(2,1);
	Matrix Excitation(2,2);

	ELevel.setEle(0,0,100);
	Excitation.setEle(0,0,.5);
	Excitation.setEle(0,1,.1);
	Excitation.setEle(1,0,.5);
	Excitation.setEle(1,1,.9);

	Matrix ThreeN(2,2);
	Matrix HundredN(2,2);
	ThreeN = Excitation.Mult(Excitation);
	ThreeN = ThreeN.Mult(Excitation);

	HundredN = Excitation.Mult(Excitation);

	for(int i=0; i<99; i++)
	{
		HundredN = HundredN.Mult(Excitation);
	}

	Matrix ThreeNResult(2,1);
	Matrix HundredNResult(2,1);

	ThreeNResult = ThreeN.Mult(ELevel);
	HundredNResult = HundredN.Mult(ELevel);

	ThreeNResult.Print();
	HundredNResult.Print();	
}