double circle(double radius, double step)
{
	double area = 0;
	for(double i=-1*radius; i<radius; i+=(2*radius/step))
		for(double k=-1*radius; k<radius; k+=(2*radius/step))
			if(i*i+k*k<=radius*radius)
				area+=(2*radius/step)*(2*radius/step);
	return area;
}

double sphere(double radius, double step)
{
	double volume=0;
	for(double x=-1*radius; x<radius; x+=(2*radius/step))
		for(double y=-1*radius; y<radius; y+=(2*radius/step))
			for(double z=-1*radius; z<radius; z+=(2*radius/step))
				if(x*x+y*y+z*z<=radius*radius)
					volume+=(2*radius/step)*(2*radius/step)*(2*radius/step);
	return volume;
}

double hyper(double radius, double step)
{
	double volume=0;
	for(double x=-1*radius; x<radius; x+=(2*radius/step))
		for(double y=-1*radius; y<radius; y+=(2*radius/step))
			for(double z=-1*radius; z<radius; z+=(2*radius/step))
				for(double m=-1*radius; m<radius; m+=(2*radius/step))
					if(x*x+y*y+z*z+m*m<=radius*radius)
						volume+=(2*radius/step)*(2*radius/step)*(2*radius/step)*(2*radius/step);
	return volume;
}