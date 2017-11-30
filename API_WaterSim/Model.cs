using System;
using System.Collections.Generic;
//using System.Text;
using System.IO;                        // for BinaryReader

using WaterSimDCDC;                     // Model proper (constructor)
using API_WaterSim;
using StreamOutputs;
using SimpleFileManager;
// 10.08.14

namespace WaterSim
{
    public class Model : WaterSimU
    {
        #region classdefinitions
        /// <summary>  Tests model parameters, variables, inputs and outputs. </summary>
        /// <remarks>   David A Sampson 11/10/2015. </remarks>
        const string _APIVersion = "9.0";  // latest version of API
        string _ModelBuild = "";
        internal WaterSimManager_SIO ws;
        internal StreamWriter sw;
        internal StreamWriter swAdd;
        DateTime now = DateTime.Now;
        //System.IO.StreamWriter MySW;
        //
        bool stopHeader = false;

        // Variables to create scenarios
        ModelParameterClass SVTraceParm;
        ModelParameterClass COTraceParm;
        ModelParameterClass RainParm;
        public ModelParameterClass POPgrowth;
        public ModelParameterClass WatEfficiency;
        //
        public int growth_ = 0;
        public int eff_ = 0;
        //
        internal FileOutputs FO;
        internal FileOutputsBase FOB;
        //
        ProviderIntArray myout = new ProviderIntArray(0);
        internal int[] OneHundred = new int[ProviderClass.NumberOfProviders];

        //
        const int FCORTraceN = 3;
        int[] COtrace = new int[FCORTraceN] { 1906, 1930, 1921 };
        //  
        const int FSVRTraceN = 3;
        int[] SVtrace = new int[FCORTraceN] { 1965, 1955, 1946 };
        const int FRainN = 4;
        int[] RainFall = new int[FRainN] { 60, 80, 100, 120 };

        //const int FCORTraceN = 1;
        //int[] COtrace = new int[FCORTraceN] { 1906 };
        ////  
        //const int FSVRTraceN = 1;
        //int[] SVtrace = new int[FCORTraceN] { 1965 };
        //const int FRainN = 1;
        //int[] RainFall = new int[FRainN] { 100 };

        //
        const int FScenario = 7;
        int[] Scenario = new int[FScenario] { 1, 2, 3, 4, 5, 6, 7 };

        string[] sScenario = new string[FScenario] { "AD", "AF", "AH", "HHH", "EN", "ZW", "BAU" };
        //string[] sScenario = new string[FScenario] { "BAU" };
        //int RecCount = 1;

        const int FGrowthN = 5;
        int[] Growth = new int[FGrowthN] { 80, 90, 100, 110, 120 };
        //const int FGrowthN = 1;
        //int[] Growth = new int[FGrowthN] {100};
        const int FEffiency = 5;
        int[] Efficiency = new int[FEffiency] { 60, 70, 80, 90, 100 };
        //const int FEffiency = 1;
        //int[] Efficiency = new int[FEffiency] {100 };
        bool isSetError = false;
        string SetErrMessage = "No Error";
        //
        #endregion
        #region constructor
        /// <summary>   Constructor. </summary>
        /// <remarks>   11/10/2015. </remarks>
        /// <param name="DataDirectoryName">    Pathname of the data directory. </param>
        /// <param name="TempDirectoryName">    Pathname of the temporary directory. </param>
        public Model(string DataDirectoryName, string TempDirectoryName)
        {
            WaterSimManager.CreateModelOutputFiles = false;
            ws = new WaterSimManager_SIO(DataDirectoryName, TempDirectoryName);
               StreamW(TempDirectoryName);
              // simpleRun();
               runWithCSV();
               // Run();
        }
        #endregion
        #region scenarios
        /// <summary>
        /// Run the model
        /// </summary>
        /// <param name="sw"></param>
        int[] GPCD = new int[33];
        public void Run()
        {
            ScnGenForm();
            myVariables my = new myVariables();
            my.myParameters();          
            FO = new FileOutputs(ws);
             SimpleFileCopy SF = new SimpleFileCopy();
            //
            String Filename = "ScenarioProject.txt";
            int ScnCnt = 1;
            double Count = 1;
            double traces = FCORTraceN * FSVRTraceN*FGrowthN*FRainN*FEffiency;
            double total = traces;// *FScenario;
            sw = new System.IO.StreamWriter(Filename);
            String File = "ByPass.txt";
            swAdd = new System.IO.StreamWriter(File);
            ProviderIntArray New = new ProviderIntArray();
            ProviderIntArray Values = new ProviderIntArray();
            for (int i = 0; i < New.Length; i++) { New[i] = 0; }
            for (int i = 0; i < New.Length; i++) { Values[i] = 50; }

            if (get_ValidModelRun == true)
            {
                // One at a time ------
                int scen = 7;
                SF.copyScenarioFile(scen);
                //
                foreach (int st in SVtrace)
                {
                    foreach (int co in COtrace)
                    {
                        foreach (int rain in RainFall)
                        {
                            foreach (int growth in Growth)
                            {
                                foreach (int eff in Efficiency)
                                {
                                    string ScnName = "Base";
                                    RunningGrowth = growth;
                                    RunningEff = eff;
                                    //if(scen >1) sScenario[scen - 1];
                                    string ver = ws.Model_Version;
                                    string buid = ws.ModelBuild;
                                    ws.Simulation_Initialize();
                                    Init();
                                    InitSpecial(New, Values);
                                    my.Initialize(scen, ws);
                                    if (SetParms(st, co, rain, growth,eff))
                                    {
                                        string Cstring = DateTime.Now.ToString("h:mm:ss tt") + " In step: " + "--" + Count + " Of: " + total + " > " + (Count / total) * 100 + " %";
                                        Console.WriteLine(Cstring);
                                        Console.WriteLine("");
                                        Console.WriteLine("Simulating " + scen + " " + ScnName);

                                        simpleRun();
                                    }
                                                  

                                    Count += 1;
                                    runOutputs(scen, ScnCnt, my, sw);
                                    ws.Simulation_Stop();
                                }
                            }
                        }
                    }
                }
            }
            CloseFiles();
            sw.Flush();
            sw.Close();
            swAdd.Flush();
            swAdd.Close();

        }
        #endregion
        public int RunningGrowth
        {
            get { return growth_; }
            set { growth_ = value; }
        }
        public int RunningEff
        {
            get { return eff_; }
            set {eff_ = value;}
        }
        #region simpleRun
        void simpleRun()
        {
            for (int year = ws.Simulation_Start_Year; year < ws.Simulation_End_Year; ++year)
            {
                ws.Simulation_NextYear();
                StartSimulation = false;
                runCSV(year);
            }
        }
        ProviderIntArray CO = new ProviderIntArray(0);
        void runWithCSV()
        {
            myVariables my = new myVariables();
            my.myParameters();
            FO = new FileOutputs(ws);
            SimpleFileCopy SF = new SimpleFileCopy();
            int scen = 1;
            SF.copyScenarioFile(scen);

            string buid = ws.ModelBuild;
            ws.Simulation_Initialize();
            Init();
            //SF.copyScenarioFile(scen);
            my.Initialize(scen, ws);
            // 10.26.17
            List<string> ProcessList = new List<string>();
            ProcessList = ws.ProcessManager.ActiveProcesses;
            //
            //isSetError = SetModelParameters(InputFields, ws, ref SetErrMessage);
            //
            //ws.Colorado_Climate_Adjustment_Percent = 100;
            //ws.SaltVerde_Climate_Adjustment_Percent = 100;
            for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 20; }
            myout.Values = OneHundred;

            ws.WaterAugmentation.setvalues(myout);

            ws.Simulation_AllYears();
            CO = ws.Colorado_Annual_Deliveries.getvalues();
            int Col = ws.Colorado_Annual_Deliveries.RegionalValue(eProvider.Regional);

            //for (int year = ws.Simulation_Start_Year; year < ws.Simulation_End_Year; ++year)
            //{
            //    ws.Simulation_NextYear();
            //    StartSimulation = false;

            //    int Rain = ws.RainWaterHarvested_MF.RegionalValue(eProvider.Regional) + ws.RainWaterHarvested_SF.RegionalValue(eProvider.Regional)
            //        + ws.RainWaterHarvested_PU.RegionalValue(eProvider.Regional);
            //    int Gray = ws.ResGrayWater.RegionalValue(eProvider.Regional);
            //    double Out = ws.PCT_Outdoor_WaterUseRes.RegionalValue(eProvider.Regional);
            //    double res = ws.PCT_WaterSupply_to_Residential.RegionalValue(eProvider.Regional);
            //    double indoor = ws.ResidentialHighDensityIndoorGPCD.RegionalValue(eProvider.Regional) +
            //        ws.ResidentialLowDensityIndoorGPCD.RegionalValue(eProvider.Regional) +
            //        ws.ResidentialMediumDensityIndoorGPCD.RegionalValue(eProvider.Regional);

            //    int odd = year;
            //    if (odd % 2 == 0)
            //    {
            //        sw.WriteLine(year
            //           + ","
            //           + ws.SaltVerde_Annual_Deliveries_SRP.RegionalValue(eProvider.Regional)
            //           +","
            //           + ws.Colorado_Annual_Deliveries.RegionalValue(eProvider.Regional)
            //           + ","
            //           + ws.Groundwater_Pumped_Municipal.RegionalValue(eProvider.Regional)
            //           + ","
            //           + ws.Groundwater_Bank_Used.RegionalValue(eProvider.Regional)
            //           + ","
            //           + ws.Reclaimed_Water_Used.RegionalValue(eProvider.Regional)
            //           + ","
            //           //+ Rain
            //           //+ ","
            //           //+ Gray
            //           + ws.RainWaterHarvtoTotalOutdoorUse.RegionalValue(eProvider.Regional)
            //           + ","
            //           + ws.GrayWaterToTotalOutdoorUse.RegionalValue(eProvider.Regional)
            //           + ","
            //           + ws.ReclaimedToTotalOutdoorUse.RegionalValue(eProvider.Regional)


            //           //+ ws.ResidentialLowDensityOutdoorGPCD.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.ResidentialLowDensityIndoorGPCD.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.LowDensityDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.MediumDensityDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.HighDensityDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.TurfWaterDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.GreenwayWaterDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.TreeWaterDemand.RegionalValue(eProvider.Regional)
            //           + ","
            //           + ws.Total_Demand.RegionalValue(eProvider.Regional)
            //           //+ ws.ResidentialMediumDensityOutdoorGPCD.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.ResidentialHighDensityOutdoorGPCD.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.Total_Demand.RegionalValue(eProvider.Regional) 
            //           //+ ","
            //           //+ ws.LowDensityDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.MediumDensityDemand.RegionalValue(eProvider.Regional)
            //           //+ ","
            //           //+ ws.HighDensityDemand.RegionalValue(eProvider.Regional)
            //           //+ ((Convert.ToDouble(ws.Total_Demand.RegionalValue(eProvider.Regional))/Convert.ToDouble( ws.Population_Used.RegionalValue(eProvider.Regional))) * 325851)/365
            //            //+ Rain
            //            //+ ","
            //            //+ Gray
            //            //+ ","
            //            //+ ws.Total_Demand.RegionalValue(eProvider.Regional) * Out / 100 * res / 100
            //          );
            //    }
            //    else
            //    {
            //          //sw.WriteLine(year
            //          //+ ","
            //          // + ws.Total_Demand.RegionalValue(eProvider.Regional)
            //          //    );

            //    }

            //}
            CloseFiles();
            sw.Flush();
            sw.Close();

        }
        void runCSV(int year)
        {
            swAdd.WriteLine(year
             + ","
             //+ RunningGrowth
             //+ ","
             //+ RunningEff
             //+ ","
             + ws.LowDensityDemand.RegionalValue(eProvider.Regional)
             + ","
             + ws.MediumDensityDemand.RegionalValue(eProvider.Regional)
             +","
             + ws.HighDensityDemand.RegionalValue(eProvider.Regional)
             + ","
             + ws.TurfWaterDemand.RegionalValue(eProvider.Regional)
             + ","
             + ws.TreeWaterDemand.RegionalValue(eProvider.Regional)

                );


        }
        #endregion
        #region Init
        void Init()
        {
            // ========================================
            parmAPIcleared = true;
            //
            ws.Colorado_Historical_Extraction_Start_Year = 1922;
            ws.SaltVerde_Historical_Extraction_Start_Year = 1946;
            //
            //set_parmIncludeMeteorology = false;
            set_parmIncludeMeteorology = true;
            ws.Simulation_End_Year = 2061;
            //     setting
            //ws.PCT_Wastewater_Reclaimed[API.ph] = 70;
            //Rec = ws.ParamManager.Model_Parameter(eModelParam.epWebReclaimedWater_PCT);
            //Rec.Value = 30;

            //ws.Colorado_Climate_Adjustment_Percent = 50;
            //ws.SaltVerde_Climate_Adjustment_Percent = 100;
            //ws.Colorado_User_Adjustment_Percent = 70;
            //ws.Colorado_User_Adjustment_StartYear = 2020;
            //ws.Colorado_User_Adjustment_Stop_Year = 2030;

            ws.ParamManager.Model_Parameter("REGRECEFF").Value = 100;
            ws.ParamManager.Model_Parameter("WEBAGTR1").Value = 100;
            // Ag efficiency
            ws.ParamManager.Model_Parameter("WEBAGEFF").Value = 90;
            //
            // ws.ParamManager.Model_Parameter("DROUSCEN").Value = 3;
            // ws.ParamManager.Model_Parameter("REGRECEFF").Value = reg;
            //   ws.ParamManager.Model_Parameter("WEBOUTDOOR").Value = 60;

            //     getting
            //.RegionalValue(eProvider.Regional)
            // + ws.Regional_Groundwater_Balance
            ws.API_Cleared = true;

        }
        // =====================================================================================
        void InitSpecial(ProviderIntArray one,ProviderIntArray two)
        {
            ws.PCT_Reclaimed_Outdoor_Use.setvalues(one);
            ws.PCT_Wastewater_Reclaimed.setvalues(one);
            ws.PCT_Reclaimed_to_Water_Supply.setvalues(two);
            ws.RainFallFactor = 100;
        }
        // =====================================================================================
        public bool SetParms(int SVTraceYr, int COTraceYr, int rain, int growth, int personal)
        {
            bool result = true;
            SVTraceParm.Value = SVTraceYr;
            COTraceParm.Value = COTraceYr;
            RainParm.Value = rain;
            POPgrowth.Value = growth;
            WatEfficiency.Value = personal;
            return result;
        }
        // =====================================================================================
        public bool SetParm(int SVTraceYr, int COTraceYr)
        {
            bool result = true;
            SVTraceParm.Value = SVTraceYr;
            COTraceParm.Value = COTraceYr;
            return result;
        }
        // =====================================================================================
        public void ScnGenForm()
        {
            ws.IncludeAggregates = true;
            SVTraceParm = ws.ParamManager.Model_Parameter(eModelParam.epSaltVerde_Historical_Extraction_Start_Year);
            COTraceParm = ws.ParamManager.Model_Parameter(eModelParam.epColorado_Historical_Extraction_Start_Year);
            RainParm = ws.ParamManager.Model_Parameter(eModelParam.epRainFallFactor);
            POPgrowth = ws.ParamManager.Model_Parameter(eModelParam.epWebPop_GrowthRateAdj_PCT);
            WatEfficiency = ws.ParamManager.Model_Parameter(eModelParam.epWebUIPersonal_PCT);
        }
        // =====================================================================================
        #endregion
        public void runOutputs(int i, int ScnCnt, myVariables mine, System.IO.StreamWriter SW)
        {
            string ScnName = "";
            ScnName = i + ",";
            //
            if (ScnCnt == 1)
            {
                if (stopHeader)
                {
                }
                else
                {
                    FO.WriteHeader(ws.SimulationRunResults,mine,  sw);
                }
                stopHeader = true;

            }
            if (FO.WriteResults(ws.SimulationRunResults, ScnName,mine, sw))
            {
                //ScnCnt++;

            }
            else
            {
            }
            ws.Simulation_Stop();
        }
        // =====================================================================================
        public void StreamW(string TempDirectoryName)
        {
            string filename = string.Concat(TempDirectoryName + "Output" + now.Month.ToString()
                + now.Day.ToString() + now.Minute.ToString() + now.Second.ToString()
                + "_" + ".csv");
            sw = File.AppendText(filename);
        }
        public string APiVersion { get { return _APIVersion; } }
        /// <summary>
        /// Verson of the Fortran Model
        /// </summary>
        public string ModelBuild { get { return _ModelBuild; } }

    }
}
