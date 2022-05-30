#include "stdafx.h"


#include <vector>
#include <string>
#include <stdio.h>  /* defines FILENAME_MAX */
#include <fstream>
#include <iostream>
#include <iomanip> 

#include "DeckWritter.h"
#include "Utils.h"

using namespace std;

bool DeckWritter::parseConfigFile(string configFile)
{
	ifstream file(configFile);
	if (!file)
	{
		return false;
		std::cout << "Cannot open the file" << std::endl;
	}

	string s;

	//first all the scalar values 
	if (Utils::findSectionInFile(file, "casename:")) file >> data._caseName;
	else { std::cout << "Cannot parse CaseName from config file" << std::endl; abort(); }


	if (Utils::findSectionInFile(file, "cells:")) for (int d = 0; d < DIMS; d++) file >> data->cells[d];
	else { std::cout << "Cannot parse cells from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "spacing:")) for (int d = 0; d < DIMS; d++) file >> data->spacing[d];
	else { std::cout << "Cannot parse spacing from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "origin:")) for (int d = 0; d < DIMS; d++) file >> data->origin[d];
	else { std::cout << "Cannot parse origin from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "minstrain:")) file >> data->minStrain;
	else { std::cout << "Cannot parse minstrain from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "maxstrain:")) { file >> data->maxStrain; }
	else { std::cout << "Cannot parse maxstrain from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "MAXSTRAINANGLE:")) { file >> data->maxStrainAngle; }
	else { std::cout << "Cannot parse MAXSTRAINANGLE from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "DirectionI:")) { for (int d = 0; d < DIMS; d++) file >> data->directionIncreaseI[d]; }
	else { std::cout << "Cannot parse DirectionI from config file" << std::endl; abort(); }
	if (Utils::findSectionInFile(file, "DirectionJ:")) { for (int d = 0; d < DIMS; d++) file >> data->directionIncreaseJ[d]; }
	else { std::cout << "Cannot parse DirectionJ from config file" << std::endl; abort(); }



	//now all the file names; 

	//the one-file keys 
	if (Utils::findSectionInFile(file, "DENSITY:")) { file >> data->_densFile; }
	else { std::cout << "Cannot parse minstrain from config file" << std::endl; abort(); }
	if (Utils::findSectionInFile(file, "YOUNGSMOD:")) { file >> data->_ymFile; }
	else { std::cout << "Cannot parse YOUNGSMOD from config file" << std::endl; abort(); }
	if (Utils::findSectionInFile(file, "POISSONR:")) { file >> data->_prFile; }
	else { std::cout << "Cannot parse POISSONR from config file" << std::endl; abort(); }
	if (Utils::findSectionInFile(file, "EDGELOAD:")) { file >> data->_edgeLoad; }
	else { std::cout << "Cannot parse EDGELOAD from config file" << std::endl; abort(); }
	if (Utils::findSectionInFile(file, "DISPLACEMENTS:")) { file >> data->_displacements; }
	else { std::cout << "Cannot parse EDGELOAD from config file" << std::endl; abort(); }


	//fileds described by multiple files 
	if (Utils::findSectionInFile(file, "PRESSURESFILES:"))
	{
		getline(file, s);
		char *x = ",";
		vector<string> tokens = Utils::tokenize2(s, x);
		for (string str1 : tokens)
		{
			str1.erase(std::remove(str1.begin(), str1.end(), ' '), str1.end());
			data._pressureFiles.push_back(str1);
		}
	}
	else { std::cout << "Cannot parse pressures from config file" << std::endl; abort(); }

	if (Utils::findSectionInFile(file, "PRESSUREDATES:"))
	{
		getline(file, s);
		char *x = ",";
		vector<string> tokens = Utils::tokenize2(s, x);
		for (string str1 : tokens)
		{
			//str1.erase( std::remove( str1.begin(), str1.end(), ' ' ), str1.end() );
			data._pressureDates.push_back(str1);
		}
	}
	else { std::cout << "Cannot parse PRESSUREDATES from config file" << std::endl; abort(); }

	file.close();
	return true;
}

bool  DeckWritter::writeDisFiles(ElementOrdering ordering)//string file, const int *cells, const double *spacing, const double *strain, ElementOrdering ordering = ElementOrdering::KJI )
{
	string fileName = writeFolder + data._caseName + "CPlusServer.dis";
	double *strain = new double[2]{ data.minStrain, data.maxStrain };
	double *spacing = &(data.spacing[0]);
	double maxStrain = max(strain[0], strain[1]);
	double minStrain = min(strain[0], strain[1]);
	double length[2] = { spacing[0] * (data.cells[0]), spacing[1] * (data.cells[1]) };






	//so, the MAXSTRAINANGLE IS MEASURED IN RESPECT TO NORTH. WHICH VECTOR POINTS NORTH HERE? I OR J 
	//THATS THE DIRECTION OF THE MAXIMUM APPLIED STRAIN. So there are several potential scenarios. 
	//FOR US, NORTH IS ALWAYS (0,1,0)
	double displacementI, visageDirectionForDisplacementI;
	double displacementJ, visageDirectionForDisplacementJ;
	int dirI = 3;
	int dirJ = 1;
	int dirK = 2;

	//so....who is pointing North ? 
	double IProjectedNorth = 0.0;
	double JProjectedNorth = 0.0;
	double north[] = { 0.0,1.0,0.0 };
	double signI = 1.0, signJ = 1.0;
	for (int d = 0;d < DIMS;d++)
	{
		IProjectedNorth += fabs((data.directionIncreaseI[d] * north[d]));
		JProjectedNorth += fabs((data.directionIncreaseJ[d] * north[d]));
	}
	if (IProjectedNorth >= JProjectedNorth) //then I is pointing north. It shoould get the max strain if angle is zero and min strain if 90. This is the FIVESIZSEVEN Example 
	{
		//strain
		displacementI = 0.5 * length[0] * (fabs(data.maxStrainAngle) < 0.001 ? maxStrain : minStrain);  //the ui only allows angles of either 0 or 90 degrees for now. 
		displacementJ = 0.5 * length[1] * (fabs(data.maxStrainAngle) < 0.001 ? minStrain : maxStrain);

		//visage direction. 
		//I know the displacements along North will need to be applied in the Petrel y direction. This is visage -z direction. Hence, dirI = 3 and sign = -1.0; 
		visageDirectionForDisplacementI = 3;

		//the other direction is Petrel x, which is x in visage. So:
		visageDirectionForDisplacementJ = 1;

		signI = -1.0;
	}
	else
	{
		//pretty much the same, but we change I and J 
		//now J is pointing North. So, if the angle is small, the max strain is poiting north, i.e. J
		//strain
		displacementJ = 0.5 * length[1] * (fabs(data.maxStrainAngle) < 0.001 ? maxStrain : minStrain);
		displacementI = 0.5 * length[0] * (fabs(data.maxStrainAngle) < 0.001 ? minStrain : maxStrain);

		//visage direction. 
		//I know the displacements along north will need to be applied in the Petrel y direction. This is visage -z direction. Hence, dirJ = 3 and sign = -1.0; 
		visageDirectionForDisplacementJ = 3;

		//the other direction is Petrel x, which is x in visage. So:
		visageDirectionForDisplacementI = 1;

		signJ = -1.0;
	}



	std::ofstream ofs(fileName, std::ofstream::out);
	if (!ofs)
	{
		std::cout << "Cannot open output file " << fileName << std::endl;
		return false;
	}

	ofs << "*DISPLACEMENTS,N" << std::endl;
	int i = 0, j = 0, element = 0;



	/*
	*
	*     Cells is re-defined to account for Ck = Ck + 1 for nodes. Then we can use the same methodology as cells
	*
	*/
	int cells[] = { 1 + data.cells[0], 1 + data.cells[1], 1 + data.cells[2] }; //nodes 
	int aux = 0;
	int counter = 0;


	int nodesDims[] = { 1 + data.cells[0], 1 + data.cells[1], 1 + data.cells[2] }; //nodes 


	//dirI = 3;
	//dirJ = 1;
	//int dirK = 1;
	/**/
	//I- and I+

	//
	//i = 0;
	//for (j = 0; j < cells[1]; j++) //i = 0;
	//	{
	//	for (int k = 0; k < cells[2]; k++)
	//		{
	//		element = getElementIndex( i, j, k, &cells[0], ordering );
	//		writeElementalDisplacement( ofs, element, dirI, 0.0001);
	//		counter++;

	//		aux += 1;
	//		}
	//	}

	//i = cells[0] - 1;
	//for (j = 0; j < cells[1]; j++) //i = 0;
	//{
	//	for (int k = 0; k < cells[2]; k++)
	//	{
	//		element = getElementIndex(i, j, k, &cells[0], ordering);
	//		writeElementalDisplacement(ofs, element, dirI, -0.0001);
	//		counter++;

	//		aux += 1;
	//	}
	//}



	//I- and I+ end 

	////J- and J+
	//j = 0;
	//for (int k = 0; k < cells[2]; k++)
	//	{
	//	for (i = 0; i < cells[0]; i++)  //	j = 0;
	//		{
	//		element = getElementIndex( i, j, k, &cells[0], ordering );
	//		writeElementalDisplacement( ofs, element, 1 /*dirJ*/, displacementJ);// displacementJ );
	//		counter++;

	//		aux += 1;

	//		std::cout << "node " << i << " 0 " << k << " " << element << std::endl;
	//		}
	//	
	//
	//
	//}

	//j = cells[1] - 1;
	//for (int k = 0; k < cells[2]; k++)
	//	{
	//	for (i = 0; i < cells[0]; i++)  //	j = 0;
	//		{
	//		element = getElementIndex( i, j, k, &cells[0], ordering );
	//		writeElementalDisplacement( ofs, element, 1/*dirJ*/, -displacementJ);// -displacementJ );
	//		counter++;

	//		aux += 1;
	//		}
	//	}//all the ks'





	//	 //this is the base 



	/*
	for (i = 0; i < nodesDims[0]; i++)  //	j = 0;
		for (j = 0; j < nodesDims[1]; j++)  //	j = 0;
		writeElementalDisplacement(ofs, getElementIndex(i, j, cells[2]-1, &cells[0], ordering), 2, 0.0);// -displacementJ );
	*/



	counter = 0;


	for (i = 0; i < nodesDims[0]; i++)  //	j = 0;
		for (j = 0; j < nodesDims[1]; j++)  //	j = 0;
			for (int k = 0; k < nodesDims[2]; k++)  //	j = 0;	
			{
				if (i == 0)
					writeElementalDisplacement(ofs, counter + 1, visageDirectionForDisplacementI, displacementI*signI);
				else if (i == cells[0] - 1)
					writeElementalDisplacement(ofs, counter + 1, visageDirectionForDisplacementI, -displacementI*signI);
				else { ; }



				if (j == 0)
					writeElementalDisplacement(ofs, counter + 1, visageDirectionForDisplacementJ, displacementJ*signJ);

				else if (j == cells[1] - 1)
					writeElementalDisplacement(ofs, counter + 1, visageDirectionForDisplacementJ, -displacementJ*signJ);
				else { ; }



				if (k == 0)
				{
					;//	writeElementalDisplacement(ofs, counter + 1, dirK, -33.0);
				}

				else if (k == cells[2] - 1)
				{
					writeElementalDisplacement(ofs, counter + 1, dirK, 0.0);
				}
				else { ; }

				//if ((i == 0) && (j == 0))
				//{
				//	writeElementalDisplacement(ofs, counter + 1, dirI, 0.0);
				//	writeElementalDisplacement(ofs, counter + 1, dirJ, 0.0);
				//}

				//if (i == cells[0] - 1)
				//{
				//	writeElementalDisplacement(ofs, counter + 1, dirI, 0.0);
				//}




				counter++;
			}





	ofs.close();


	std::cout << "Wrote " << aux << " displacements. Cells=  " << cells[0] << " " << cells[1] << " " << cells[2] << std::endl;
	return true;
}



bool  DeckWritter::writeEdgeLoadFile()
{
	string fileName = writeFolder + data._caseName + ".edg";
	int *cells = &(data.cells[0]);

#ifdef _WINDOWS
	string sep = "\\";
#else
	string sep = "/";
#endif

	//the edge loads as written as floats in binary format, one after another in kji order. only k = 0 is subject to load  
	string inputFileName = readFolder + "BOUNDARIES" + sep + data._edgeLoad;// 
	std::cout << "Reading from file  " << inputFileName << std::endl;
	ifstream inputFile(inputFileName, ios::binary);
	if (!inputFile)
	{
		std::cout << "Cannot open input file " << inputFileName << std::endl;
		return false;
}

	//total number of floats = CI*CJ; 
	int chunkSize = cells[0] * cells[1];
	float *values = new float[chunkSize];
	inputFile.read((char*)(values), chunkSize * sizeof(float));

	std::ofstream ofs(fileName, std::ofstream::out);
	if (!ofs)
	{
		std::cout << "Cannot open output file " << fileName << std::endl;
		return false;
	}
	ofs << "*EDGELOADS" << std::endl;
	ofs << chunkSize << std::endl;

	int k = 0, counter = 0;
	for (int i = 0; i < cells[0];i++)
	{
		for (int j = 0; j < cells[1];j++)
		{
			ofs << (1 + counter) << "          4" << std::endl;
			int A = getFirstPointIndexOfElementIndex(i, j, k, cells);
			int B = A + ((cells[2] + 1)*(cells[1] + 1));
			int D = A + (cells[2] + 1);
			int C = B + (cells[2] + 1);

			ofs << A << "  " << B << "  " << C << "   " << D << std::endl;
			ofs << "0.0    0.0     0.0    0.0" << std::endl;
			ofs << "0.0    0.0     0.0    0.0" << std::endl;

			ofs << std::setprecision(5)
				<< values[counter] * oceanPressureToVS << "   ";
			ofs << values[counter] * oceanPressureToVS << "   ";
			ofs << values[counter] * oceanPressureToVS << "   ";
			ofs << values[counter] * oceanPressureToVS << std::endl;
			counter += 1;

		}

	}

	delete[] values;
	return true;

}

bool  DeckWritter::writePressureFiles()
{
#ifdef _WINDOWS 
	string sep = "\\";
#else 
	string sep = "/";
#endif 

	std::wcout << "Writting pressure files.....";

	for (int n = 0; n < (int)data._pressureFiles.size();n++)
	{
		string inputFileName = readFolder + "PRESSURES" + sep + data._pressureFiles[n];
		std::cout << "Reading from file  " << inputFileName << std::endl;
		ifstream inputFile(inputFileName, ios::binary);
		if (!inputFile)
		{
			std::cout << "Cannot open input file " << inputFileName << std::endl;
			return false;
		}

		string outputFileName = writeFolder + sep + data._pressureFiles[n];
		std::cout << "Re-writting pressue in file " << outputFileName << std::endl;
		ofstream outputFile(outputFileName, ios::out);
		if (!outputFile)
		{
			std::cout << "Cannot open output file " << outputFileName << std::endl;
			return false;
		}


		int counter = 1;
		int *cells = &(data->cells[0]);
		int nCells = cells[0] * cells[1] * cells[2];
		outputFile << "*POREPRESSURES,NOCOM,NO_ELE" << std::endl;
		int chunkSize = nCells / 4;
		float *values = new float[chunkSize];

		for (int k = 0; k < 4; k++)
		{
			if (k == 3)
			{
				chunkSize = nCells - 3 * chunkSize;
				delete[] values;
				values = NULL;
				values = new float[chunkSize];
				float *values = new float[chunkSize];
			}

			inputFile.read((char*)(values), chunkSize * sizeof(float));

			for (int nn = 0;nn < chunkSize;nn++)
				outputFile << /*oceanPressureToVS**/values[nn] << std::endl;
			//outputFile << (counter++) << '\t' << values[nn] << std::endl;
			std::wcout << "\rWritting pressure files....." << (100.0*(1 + k) / 4) << std::endl;
		}


		delete[] values;
		inputFile.close();
		outputFile.close();




	} //pressures


	return true;
};

bool  DeckWritter::writeMatFile()
{
#ifdef _WINDOWS 
	string sep = "\\";
#else 
	string sep = "/";
#endif 

	string outputFileName = writeFolder + sep + data._caseName + ".mat";
	ofstream outputFile(outputFileName, ios::out);
	if (!outputFile)
	{
		std::cout << "Cannot open output file " << outputFileName << std::endl;
		return false;
	}
	outputFile.close();


	appendElasicData(); //appends data 
	appendStrengthData();
	appendInertialData();

	std::cout << "Material writting finished." << std::endl;

	return true;
}

bool DeckWritter::appendStrengthData()
{
#ifdef _WINDOWS 
	string sep = "\\";
#else 
	string sep = "/";
#endif 

	int *cells = &(data->cells[0]);
	int nCells = cells[0] * cells[1] * cells[2];
	string outputFileName = writeFolder + sep + data._caseName + ".mat";
	std::cout << "Writting mat file " << outputFileName << std::endl;
	ofstream outputFile(outputFileName, ios::app);
	if (!outputFile)
	{
		std::cout << "Cannot open output file as --append--" << outputFileName << std::endl;
		return false;
	}

	outputFile << "\n\n*YIELD_DATA, NOCOM" << std::endl;
	outputFile << nCells << "  [0]" << std::endl;

	return true;
}

bool DeckWritter::appendInertialData()
{
#ifdef _WINDOWS 
	string sep = "\\";
#else 
	string sep = "/";
#endif 

	string outputFileName = writeFolder + sep + data._caseName + ".mat";
	std::cout << "Writting mat file " << outputFileName << std::endl;
	ofstream outputFile(outputFileName, ios::app);
	if (!outputFile)
	{
		std::cout << "Cannot open output file as --append--" << outputFileName << std::endl;
		return false;
}

	string densityFile = readFolder + sep + "MATERIALS" + sep + data._densFile;
	ifstream inputFile1(densityFile, ios::in | ios::binary);
	if (!inputFile1)
	{
		std::cout << "Cannot open input file " << densityFile << std::endl;
		return false;
	}

	int nCells = (data->cells[0]) * (data->cells[1]) * (data->cells[2]);

	outputFile << "\n\n*SOLID_UNIT_W,NOCOM" << std::endl; //, COMP
	outputFile << nCells << "  [25.0]" << std::endl;


	outputFile << "\n\n*SOLID_MASS_DENSITY" << std::endl; //, COMP
	outputFile << nCells << "  [2.5]" << std::endl;


	/*
	outputFile << "\n\n*SOLID_MASS_DENSITY" << std::endl; //, COMP
	int nCells = (data->cells[0]) * (data->cells[1]) * (data->cells[2]);
	int chunkSize = (int)(nCells / 4);
	float *value1 = new float[chunkSize];
	for (int k = 0; k < 4; k++)
		{
		if (k == 3)
			{
			delete[] value1;
			value1 = NULL;
			value1 = new float[nCells - 3 * chunkSize];
			chunkSize = nCells - 3 * chunkSize;
			}

		inputFile1.read( (char*)(value1), chunkSize * sizeof( float ) );
		for (int nn = 0;nn < chunkSize;nn++)
		outputFile << "1"<< '\t' << value1[nn]*oceanDensityToVS << std::endl;

		std::wcout << "\rWritting SOLID_MASS_DENSITY files....." << (100.0*(1 + k) / 4) << std::endl;
		}//kk


	//outputFile << "*SOLID_UNIT_W, NOCOM" << std::endl;
	//outputFile << nCells << "  [22.6]" << std::endl;
	delete[] value1;
	*/

	return true;
}

bool  DeckWritter::appendElasicData()
{
#ifdef _WINDOWS 
	string sep = "\\";
#else 
	string sep = "/";
#endif 

	string outputFileName = writeFolder + sep + data._caseName + ".mat";
	std::cout << "Writting mat file " << outputFileName << std::endl;
	ofstream outputFile(outputFileName, ios::app);
	if (!outputFile)
	{
		std::cout << "Cannot open output file as --append--" << outputFileName << std::endl;
		return false;
	}

	//input files are the binary files with contiguous floats. 
	string elasticModulusFile = readFolder + sep + "MATERIALS" + sep + data._ymFile;
	ifstream inputFile1(elasticModulusFile, ios::in | ios::binary);
	if (!inputFile1)
	{
		std::cout << "Cannot open input file " << elasticModulusFile << std::endl;
		return false;
	}

	string prFile = readFolder + sep + "MATERIALS" + sep + data._prFile;
	ifstream inputFile2(prFile, ios::in | ios::binary);
	if (!inputFile2)
	{
		std::cout << "Cannot open input file " << prFile << std::endl;
		return false;
	}


	int counter = 1;
	int *cells = &(data->cells[0]);
	int nCells = cells[0] * cells[1] * cells[2];
	outputFile << "*ELASTIC_DATA, NOCOM" << std::endl; //, COMP
	int chunkSize = (int)(nCells / 4);

	//because the arrays are so big, we read them and write them in chunks. We split the total number of values in 4 chunks 
	//VS does not allow me to have such massive arrays in Windows. 
	float *value1 = new float[chunkSize];
	float *value2 = new float[chunkSize];
	for (int k = 0; k < 4; k++)
	{
		//the size of thspecial adjustment because the total size may not be a multiple of four. 
		//assume size = 27 ->chunksize =27/4  = 6. so chunks 0,1 and 2 will have size = 6. the last one 27 -3x(6) =  9
		if (k == 3)
		{
			delete[] value1;
			delete[] value2;
			value1 = NULL;
			value2 = NULL;
			value1 = new float[nCells - 3 * chunkSize];
			value2 = new float[nCells - 3 * chunkSize];
			chunkSize = nCells - 3 * chunkSize;
		}
		
		/////////////////////////////////////////FOR ANDREW///////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//read in one shot all the floats for YM as binary floats 
		//we just refill the arrays allocated before.
		inputFile1.read((char*)(value1), chunkSize * sizeof(float));
		
		//read the flaots for the PR as binary float 
		inputFile2.read((char*)(value2), chunkSize * sizeof(float));

		//then write them one by one as text s visage requires.  
		//the text can be binarized, but still is text, still is an extra step and a very costly one.
		//thats is what RG does. The same text is binarized before writting. 
		for (int nn = 0;nn < chunkSize;nn++)
		outputFile << "1"/*(counter++)*/ << '\t' << /*oceanPressureToVS**/value1[nn] << '\t' << value2[nn] << std::endl;
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////



		counter += 1;
		std::wcout << "\rWritting ELASTIC_DATA .....progress" << (100.0*(1.0 + k) / 4.0) << std::endl;
	}//kk



	delete[] value1;
	delete[] value2;

	outputFile << "\n\n*BIOTS_MODULUS, NOCOM" << std::endl;
	outputFile << nCells << "  [1]" << std::endl;

	return true;
}

bool  DeckWritter::writeDeck(string configFile)
{

	bool success = true;
	setConfigFile(configFile);

	if (!parseConfigFile(configFile))
	{
		std::cout << "Cannot parse the config file" << std::endl;return false;
	}
	std::cout << "Info Parsed so far from the config file: \n" << data << std::endl;

	if (!writeDisFiles()) // &(data.cells[0]), &(data.spacing[0]), &(strains[0]) );//, ElementOrdering ordering = ElementOrdering::KJI )
	{
		std::cout << "Error when writting dis file" << std::endl;return false;
	}

	return true;


	
	if (!writeEdgeLoadFile())
	{
		std::cout << "Error when writting edge-load file" << std::endl;return false;
	}
	if (!writePressureFiles())  //this right now a simple binary-text converter with an special keyword 
	{
		std::cout << "Error when writting pressure files" << std::endl;return false;
	}

	if (!writeMatFile())
	{
		std::cout << "Error when writting mat file" << std::endl;return false;
	}
	


	return success;
}


