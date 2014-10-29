//Imran Yafai
//Comp Phys Project 8C
//.25	.5	.75	1	1.25	1.5
//.7	.88	1.15	1.54	1.81	2.09

#include "Matrix.h"
#include <math.h>

int main()
{
	Matrix M(6,2);
	Matrix B(6,1);
	for(double i=0; i<1.5; i+=.25)
	{
		M.setEle(i*4,0, i+.25);
		M.setEle(i*4,1,1);
	}

	B.setEle(0,0,.7);
	B.setEle(1,0,.88);
	B.setEle(2,0,1.15);
	B.setEle(3,0,1.54);
	B.setEle(4,0,1.81);
	B.setEle(5,0,2.09);

	M.Print();

	Matrix MT(2,6);
	MT = M.Transpose();
	MT.Print();

	Matrix MTM(2,2);
	Matrix MTMIMT(2,6);

	MTM = MT.Mult(M);

//	MTM.Print();

	Matrix MTMI(2,2);
	MTMI = MTM.Invert();
	MTMI.Print();

	(MTMI.Mult(MT)).Print();
	
	Matrix zclosest(2,1);

	MTMIMT = MTMI.Mult(MT);
	cout<<"Z_closest = "<<endl;
	zclosest = MTMIMT.Mult(B);
	zclosest.Print();
	
}