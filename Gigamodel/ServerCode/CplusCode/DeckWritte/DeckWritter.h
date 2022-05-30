#ifndef _DECK_WRITTER_H_
#define _DECK_WRITTER_H_ 1


#include <vector>
#include <string>
#include <stdio.h>  /* defines FILENAME_MAX */
#include <fstream>

#include <future>
#include <numeric>

#include "Definitions.h"
#include "Utils.h"


using namespace std;



enum ElementOrdering { KJI, IJK };


class DataFile
	{

	//helper operator overload to write the object just as a basic type, same notation. 
	friend std::ostream& operator<<( std::ostream &out, const DataFile &data )
		{
		out << "\n Configfile: " << data.configFile << std::endl;

		out << "\n CaseName: " << data._caseName << std::endl;
		out << "\n Cells: ";
		for (int i : data.cells)    out << i << ' ';

		out << "\n Spacing: ";
		for (auto f : data.spacing) out << f << ' ';

		out << "\n Origin: ";
		for (auto f : data.origin) out << f << ' ';

		out << "\n Strain: ";
		out << data.minStrain << " " << data.maxStrain << "  " << data.maxStrainAngle << std::endl;

		out << "\n Pressures:" << data._pressureFiles.size() << std::endl;
		for (auto s : data._pressureFiles) 	 out << '\t' << s << std::endl; out << '\n';

		out << "\n Dates:" << data._pressureDates.size() << std::endl;
		for (auto s : data._pressureDates) 	 out << '\t' << s << std::endl; out << '\n';

		out << "\n YM:" << data._ymFile << std::endl;
		out << "\n PR:" << data._prFile << std::endl;
		out << "\n RHO:" << data._densFile << std::endl;
		out << "\n EDGELOAD:" << data._edgeLoad << std::endl;
		out << "\n DISPLACEMENTS:" << data._displacements << std::endl;



		return out;

		}

	public:

		DataFile()
			{
			maxStrainAngle = maxStrain = minStrain = 0.10;
			cells[2] = 1;
			}

		int    cells[DIMS];
		double spacing[DIMS];
		double origin[DIMS];
		double directionIncreaseI[DIMS];
		double directionIncreaseJ[DIMS];


		double minStrain, maxStrain, maxStrainAngle; //angle respect to north in the UI

		vector<string>  _pressureFiles;
		vector<string>  _pressureDates;
		string  _ymFile;
		string  _prFile;
		string  _densFile;
		string  _edgeLoad;
		string  _displacements;

		string _caseName;
		string configFile;

		DataFile* operator->() { return this; }

	};



class DeckWritter
	{
	friend std::ostream& operator<<( std::ostream& out, DeckWritter& w )
		{


		return out;
		}

	public:

		DataFile data;
		string writeFolder;
		string readFolder;


		DeckWritter()
			{
			oceanPressureToVS = 0.001;
			oceanDensityToVS = 0.001; 
			}

		DeckWritter* operator->() { return this; }

		void setConfigFile( string configFile )
			{
			data.configFile = configFile;
			string folder, file;
			Utils::getFolderAndFile( configFile, writeFolder, data.configFile );

			#ifdef _WINDOWS
			writeFolder += "\\";
			readFolder = writeFolder + "PreDeck\\";
			#else 
			writeFolder += "\\";
			readFolder = writeFolder + "PreDeck\\";
			#endif 
			}

		bool  writeDeck( string configFile );

		//at this point we just grab the pressure files, read them all as binary and re-write them as text with the appropiate keyword thats pretty much it. 
		bool writePressureFiles();

		bool writeDisFiles( ElementOrdering ordering = ElementOrdering::KJI );

		bool writeEdgeLoad();

		bool writeMatFile();
		bool appendElasicData();
		bool appendStrengthData();
		bool appendInertialData();



		

		bool writeEdgeLoadFile();

		bool parseConfigFile( string configFile );

		double oceanPressureToVS;
		double oceanDensityToVS;


	private:

		static void writeElementalDisplacement( ofstream &out, const int element, const int direction, double displacement )
			{
			out << element << '\t' << direction << '\t' << displacement << std::endl;
			}

		static inline int getElementIndex( int i, int j, int k, const int *cells, ElementOrdering ordering = ElementOrdering::KJI )
			{

			//for(i for j for k)
			return 1 + k + j*cells[2] + i*cells[2] * cells[1];


			//return 1 + i + j*cells[0] + cells[0] * cells[1] * k;

			//if (ordering == ElementOrdering::KJI)
			//{
			//	return 1 + i + cells[0] * cells[2] * j + k*cells[0];
			//}
			//else 
			//{
				;
			//}

			//return  1 + k + j * cells[2] + i*cells[2] * cells[1];

			//return ordering == ElementOrdering::KJI ? 1 + k + j * cells[2] + i*cells[2] * cells[1] : //kji
			//	1 + i + j * cells[0] + k*cells[0] * cells[1];  //this is ijk 
  //implement a switch if we add more orderings. 
			}

		static inline int getFirstPointIndexOfElementIndex( int i, int j, int k, const int *cells, ElementOrdering ordering = ElementOrdering::KJI )
			{
//			return ordering == ElementOrdering::KJI ? 1 + k + j * (1 + cells[2]) + i*((1 + cells[2]) * (1 + cells[1])) : //kji
			
	    	return ordering == ElementOrdering::KJI ? 1 + k + j * ( cells[2]) + i*((/*1 +*/ cells[2]) * (/*1 +*/ cells[1])) : //kji

				/*i + j * cells[0] + k*cells[0] * cells[1]*/999999999;  //this is ijk 
														   //implement a switch if we add more orderings. 
			}



	};

#endif 
