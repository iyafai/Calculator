//Imran Yafai
//Computational Physics Project 5C

#include <iostream>
#include <stdio.h>

#define k 8987551787.368
#define G 0.0000000000667398

using namespace std;

double sphereE(double chargeVol, double radius, double distance, double step)
{
	double field=0;
	if(distance<= radius)
		radius = distance;

	for(double x=-1*radius; x<radius; x+=(2*radius/step))
		for(double y=-1*radius; y<radius; y+=(2*radius/step))
			for(double z=-1*radius; z<radius; z+=(2*radius/step))
				if(x*x+y*y+z*z<=radius*radius)
				{
					field+=chargeVol*k*(2*radius/step)*(2*radius/step)*(2*radius/step)
					*(distance+x)/(((distance+x)*(distance+x)+y*y+z*z)
					*sqrt((distance+x)*(distance+x)+y*y+z*z));
				}
	return field;
}

double GravY(double density, double hlength, double step)
{
	double field=0;
	double offset = .000000001;

	for(double x=0; x<2*hlength; x+=(hlength/step))
		for(double y=0; y<2*hlength; y+=(hlength/step))
			for(double z=0; z<2*hlength; z+=(hlength/step))
			{
				field+=density*G*(hlength/step)*(hlength/step)*(hlength/step)
				*y/(((x-hlength+offset)*(x-hlength+offset)+y*y+(z-hlength+offset)*(z-hlength+offset))
				*sqrt((x-hlength+offset)*(x-hlength+offset)+y*y+(z-hlength+offset)*(z-hlength+offset)));
			}
	return field;
}

double GravX(double density, double hlength, double step)
{
	double field=0;
	double offset = .000000001;

	for(double x=0; x<2*hlength; x+=(hlength/step))
		for(double y=0; y<2*hlength; y+=(hlength/step))
			for(double z=0; z<2*hlength; z+=(hlength/step))
			{
				field+=density*G*(hlength/step)*(hlength/step)*(hlength/step)*(x-hlength)
				/(((x-hlength+offset)*(x-hlength+offset)+y*y+(z-hlength+offset)*(z-hlength+offset))
				*sqrt((x-hlength+offset)*(x-hlength+offset)+y*y+(z-hlength+offset)*(z-hlength+offset)));
			}
	return field;
}

double GravZ(double density, double hlength, double step)
{
	double field=0;
	double offset = .000000001;

	for(double x=0; x<2*hlength; x+=(hlength/step))
		for(double y=0; y<2*hlength; y+=(hlength/step))
			for(double z=0; z<2*hlength; z+=(hlength/step))
			{
				field+=density*G*(hlength/step)*(hlength/step)*(hlength/step)*(z-hlength)/
				(((x-hlength+offset)*(x-hlength+offset)+y*y+(z-hlength+offset)*(z-hlength+offset))
				*sqrt((x-hlength+offset)*(x-hlength+offset)+y*y+(z-hlength+offset)*(z-hlength+offset)));
			}
	return field;
}

int main()
{
	double radius=1, distance=0.25, step=100, chargeVol=2;
	double hlength=6378100, distance2=hlength/2, density=2877.19738;

	cout<<"distance, E(x)"<<endl;
	for(double i=0; i<3.1; i+=.1)
		cout<<i<<","<<sphereE(chargeVol, radius, i, step)<<endl;

	cout<<"G field at center of one face on square earth with side length "<<2*hlength<<" m and density "<<density;
	cout<<" kg/m^3"<<endl;
	cout<<"Y component: "<<GravY(density, hlength, step)<<endl;
	cout<<"X component: "<<GravX(density, hlength, step)<<endl;
	cout<<"Z component: "<<GravZ(density, hlength, step)<<endl;


}