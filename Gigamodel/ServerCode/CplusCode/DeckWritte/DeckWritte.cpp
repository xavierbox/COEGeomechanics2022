// DeckWritte.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include "Definitions.h"
#include "Utils.h"
#include "DeckWritter.h"


using namespace std;



int main(int argc, char **argv)
{

	//std::cout << "argc = " << argc << std::endl;
	//std::cout << "argv = " << argv[1] << std::endl;

	string configFile;
	if (argc < 2)
		{
		//configFile = "D:\\AppData\\GigaModel\\Simulations\\Sim1\\Sim1.cfg";
		//configFile = "D:\\AppData\\GigaModel\\Simulations\\LITTLESTRAIN\\LITTLESTRAIN.cfg";
		//configFile = "D:\\AppData\\GigaModel\\Simulations\\TestExportImpor1\\TestExportImpor1.cfg";
		
		configFile = "D:\\AppData\\GigaModel\\Simulations\\highstrain1\\highstrain1.cfg";

		configFile = "D:\\AppData\\GigaModel\\Simulations\\largerModel\\largerModel.cfg";


		configFile = "D:\\AppData\\GigaModel\\Simulations\\LITTLESTRAIN\\LITTLESTRAIN.cfg";

		configFile = "D:\\AppData\\GigaModel\\Simulations\\BENCHMARK1SIMULATION\\BENCHMARK1SIMULATION.cfg";


		configFile = "D:\\AppData\\GigaModel\\Simulations\\SIM456\\SIM456.cfg";


		configFile = "D:\\AppData\\GigaModel\\Simulations\\SINGLEMATERIAL90DEGS\\SINGLEMATERIAL90DEGS.cfg";


		configFile = "D:\\AppData\\GigaModel\\Simulations\\NapoShalyLimestone\\NapoShalyLimestone.cfg";

		configFile = "D:\\AppData\\GigaModel\\Simulations\\Sim30Million\\Sim30Million.cfg";


		configFile = "D:\\AppData\\GigaModel\\Simulations\\SIMULATION180MM\\SIMULATION180MM.cfg";

 

		configFile = "D:\\AppData\\GigaModel\\Simulations\\TEST1MILLIONS\\TEST1MILLIONS.cfg";


		configFile = "C:\\GigaModel\\Simulations\\testdis\\testdis.cfg";

 


		std::cout << "Debugging. Using the default config file if present"; 

		}
	else
		configFile = string( argv[1] );//"D:\\AppData\\GigaModel\\Simulations\\Case1\\Case1.cfg";

	
	string file, folder;
	Utils::getFolderAndFile ( configFile, folder, file ); 
	std::cout << "Folder: " << folder << "\nFile " << file << std::endl;

	DeckWritter w; 

	w.writeDeck( configFile );
	
	return 0;
}

