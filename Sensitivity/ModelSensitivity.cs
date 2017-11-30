using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterSimDCDC;
using StreamOutputs;

namespace Sensitivity
{
    class InternalModelSensitivity : WaterSimU
    {
        internal WaterSimManager_SIO ws;

        public InternalModelSensitivity(string DataDirectoryName, string TempDirectoryName)
        {
            //
            WaterSimManager.CreateModelOutputFiles = false;
            // ws.FortranDllFileName = WaterSimDCDC.WaterSimManager.FortranDll.WaterSimDCDC_model_6;
            //        
            ws = new WaterSimManager_SIO(DataDirectoryName, TempDirectoryName);
            //
           // InternalSensitivity();
        }
        public void InternalSensitivity()
        {
            ws.Simulation_Initialize();
            ws.Simulation_End_Year = 2019;
            set_parmIncludeMeteorology = true;
            parmAPIcleared = true;
            RunManyYears();
            StartSimulation = false; // have I hardcoded this in the runManyYears FORTRAN loop? CHECK......08.11.16
            //CloseFiles();
        }
 
    }
    class ExternalModelSensitivity : WaterSimU
    {
        internal WaterSimManager_SIO ws;
        internal FileOutputsBase FOB;
        //
        ModelParameterClass WAugParm;
        ModelParameterClass WWtoEff;
        ModelParameterClass WWtoReclaimed;
        ModelParameterClass RtoRO;
        //
        ModelParameterClass EffToVadose;
        ModelParameterClass EffToPower;
        //
        ModelParameterClass TimeLag;
        ModelParameterClass ModNormal;
        ModelParameterClass SurfToVadose;
        ModelParameterClass SurfToWBpct;
        ModelParameterClass WStoDI;
        //
        ModelParameterClass SdefaultPump;
        ModelParameterClass WebPopGrRate;
        ModelParameterClass GrRateOnProject;
        ModelParameterClass GrRateOffProject;
        ModelParameterClass WebPrUse;
        ModelParameterClass WebGPCD;
        ModelParameterClass GPCDmin;
        ModelParameterClass STVtrace;
        ModelParameterClass STVClimate;
        ModelParameterClass STVDroughtStart;
        ModelParameterClass STVDroughtStop;
        ModelParameterClass STVDroughtPCT;
        ModelParameterClass COTrace;
        ModelParameterClass COClimate;
        ModelParameterClass CODroughtStart;
        ModelParameterClass CODroughtStop;
        ModelParameterClass CODroughtPCT;
        ModelParameterClass VadoseLag;
        ModelParameterClass RECtoVadose;
        ModelParameterClass RECtoDI;
        ModelParameterClass RECtoWS;
        ModelParameterClass MaxREC;
        ModelParameterClass RECoutdoors;
        ModelParameterClass ROtoWS;
        //
        ModelParameterClass WSRES;
        ModelParameterClass WSCOM;
        ModelParameterClass WSIND;
        ModelParameterClass RESOUT;
        ModelParameterClass COMOUT;
        ModelParameterClass INDOUT;


        // ============================================================================

       // boolean
        ModelParameterClass AWSlimit;
       
        MyParameterList pl ;

        //const int FPCTpumpDefault = 5;
        //int[] DefaultPumping = new int[FPCTpumpDefault] { 1, 2, 3, 4, 5 };
        const int OutPutParamN=1;
        int[] eModelParametersForOutput = new int[OutPutParamN]
          {
           eModelParam.epGroundwater_Pumped_Municipal

          };

        internal FileOutputs FO;
        System.IO.StreamWriter MySW;
        public ExternalModelSensitivity(string DataDirectoryName, string TempDirectoryName)
        {
            //
             pl = new MyParameterList();

            WaterSimManager.CreateModelOutputFiles = false;
            // ws.FortranDllFileName = WaterSimDCDC.WaterSimManager.FortranDll.WaterSimDCDC_model_6;
            //        
            ws = new WaterSimManager_SIO(DataDirectoryName, TempDirectoryName);
            //
            Externalsensitivity();
            scenarios(ws);
            //aList = new List<int>();
            //int min = 0; int max = 100; int start = 0; int inc = 10;
            //Recursive(min, max, start,inc);
        }
        #region Sensitivity Analyses
        #endregion

        public void myParameters()
        {
            FOB = new FileOutputsBase();
            FOB.myParms.Add(eModelParam.epSaltVerde_Historical_Extraction_Start_Year);
            FOB.myParms.Add(eModelParam.epColorado_Historical_Extraction_Start_Year);
            FOB.myParms.Add(eModelParam.epGroundwater_Pumped_Municipal);
            FOB.myParms.Add(eModelParam.epGroundwater_Balance);
            FOB.myParms.Add(eModelParam.epGroundwater_Bank_Balance);
            FOB.eModelParametersForOutput = new int[FOB.myParms.Count];
            for (int i = 0; i < FOB.myParms.Count; i++)
            {
                FOB.eModelParametersForOutput[i] = FOB.myParms[i];
            }
        }
        //======================================================================================
        int _runningCount=0;
        public int runningCount
        {
            get {return _runningCount;}
            set {_runningCount = value;}
        }
         int _countGeneric = 0;
        public int countGeneric
        {
            get { return _countGeneric; }
            set { _countGeneric = value; }
        }
        bool _readyToRun = false;
        public bool readyToRun
        {
            get { return _readyToRun; }
            set { _readyToRun = value; }
        }
       // ModelParameterClass Test;
        public List<MyParameterInfo> NewList { get; set; }
        public void scenarios(WaterSimManager_SIO ws)
        {
            int ScnCnt = 1;
            DateTime value = new DateTime(2016, 8, 10);
            String Filename = "Sensitivity.txt";
            MySW = new System.IO.StreamWriter(Filename);
           
           // int Count = 0;
            ScnGenForm();
            myParameters();
            FO = new FileOutputs(ws);
            int Count = 0;
            //
            MyParameterInfo MP = new MyParameterInfo();
            if (get_ValidModelRun == true)
            {
                //
                string[] MyWaterUseFlds = new string[ws.WaterUseGroup.Count];
                string[] MyEffluentFlds = new string[ws.EffluentGroup.Count];
                string[] MyReclaimedFlds = new string[ws.ReclaimedGroup.Count];
                //
                foreach (MyParameterInfo MPC in pl.myList)
                {
                    ModelParameterClass TheMP = ws.ParamManager.Model_Parameter(MPC.Fieldname);
                    //              
                    if (ws.WaterUseGroup.isMember(TheMP.ModelParam))
                    {
                        //GenericCreateCountList(MPC, MyWaterUseFlds, ws.WaterUseGroup.Count);
                        //if (readyToRun)
                        //{
                        //    RunSpecial(aValuesList, ws.WaterUseGroup.Count, MyWaterUseFlds, ws, MPC);
                        //}
                     }
                    else if (ws.EffluentGroup.isMember(TheMP.ModelParam))
                    {
                        //GenericCreateCountList(MPC, MyEffluentFlds, ws.EffluentGroup.Count);
                        //if (readyToRun)
                        //{
                        //    RunSpecial(aValuesList, ws.EffluentGroup.Count, MyEffluentFlds, ws, MPC);
                        //}                 
                     }
                    else if (ws.ReclaimedGroup.isMember(TheMP.ModelParam))
                    {
                        //GenericCreateCountList(MPC, MyReclaimedFlds, ws.ReclaimedGroup.Count);
                        //if (readyToRun)
                        //{
                        //    RunSpecial(aValuesList, ws.ReclaimedGroup.Count, MyReclaimedFlds, ws, MPC);
                        //}
                    }
                    else
                    {

                        if (0 < MPC.MinIntValue) { Count = 1; }
                        for (int i = MPC.MinIntValue; i < MPC.MaxIntValue + 1; i += MPC.RangeInterval * Count - i)
                        {
                            // ws.Simulation_Initialize();
                            set_parmIncludeMeteorology = false;
                            if (parameterCheck(MPC, i))
                            {
                                //  runOnly();
                            }

                            Count += 1;
                            // runOutputs(MPC, i, ScnCnt);

                            ScnCnt++;
                        }
                        Count = 0;
                    }

                };
                
            }
            MySW.Flush();
            MySW.Close();
        }
        //public List<int[]> aValuesList = new List<int[]>();
        public List<int[]> aValuesList;
        public List<int> aList;
        public int Recursive(int min, int max, int y, int inc)
        {

            if (max < y)
                return y;
            else
            {
                Recursive(min, max, y + inc, inc);
                aList.Add(y = min + y);
                return y;
            }
        }
        public void GenericCreateCountList(MyParameterInfo MPC, string[] MyGroup, int GroupCount)
        {
            int start = 0;
            if (MyGroup[0] == null)
            {
                countGeneric = 0;
                // Create an array of values to run
                aList = new List<int>();
                Recursive(MPC.MinIntValue, MPC.MaxIntValue, start, MPC.RangeInterval);
            }
            if (MPC.Done == true)
            {
            }
            else
            {
                MyGroup[countGeneric] = MPC.Fieldname;
                MPC.Done = true;

                countGeneric += 1;
            }

            if (countGeneric == GroupCount)
            {
                // create all possible combinaitons of the values among players
                int[] MyValues = aList.ToArray();
                int[] temp = new int[MyGroup.Length];
                aValuesList = new List<int[]>();
                //MPC._recursive(MyWaterUseFlds,MyValues,temp,0);
                MPC._Recursive(MyGroup, MyValues, temp, 0, aValuesList);
                // Run the model 
                readyToRun = true;
            }

        }

  


        // ====================================================================================================================================
        //
        public void RunSpecial(List<int[]> RunList,int totalCount, string[] flds, WaterSimManager_SIO ws, MyParameterInfo MPC)
        {
            int set = 0;
            int count = 0;
            ModelParameterBaseClass MP;
            foreach (var value in RunList)
            {
                ws.Simulation_Initialize();
                set_parmIncludeMeteorology = false;
                //
                foreach (int emp in ws.ParamManager.eModelParameters())
                {
                    //MP = ws.ParamManager.Model_ParameterBaseClass(emp);
                    MP = ws.ParamManager.Model_Parameter(emp);
                    if(MP.Fieldname == flds[count])
                    {
                        parameterZeroOut(MP,MPC);
                        count++;
                        if (count == totalCount) break;                  
                    }
                }
                count = 0;
                foreach (int emp in ws.ParamManager.eModelParameters())
                {
                    MP = ws.ParamManager.Model_ParameterBaseClass(emp);
                    if (MP.Fieldname == flds[count])
                    {
                        parameterSet(MP,MPC, value[count]);
                        count++;
                        if (count == totalCount) break;
                    }
                }
                //
                 // runOnly();
                  set++;
               // runOutputs(MPC, set, runningCount);
                runningCount++;
                readyToRun = false;
                break;
            }
        }
        //
        //public void runOutputs(int i, int ScnCnt, myVariables mine, System.IO.StreamWriter SW)
        //{
        //    string ScnName = "";
        //    ScnName = i + ",";
        //    //
        //    if (ScnCnt == 1)
        //    {
        //        if (stopHeader)
        //        {
        //        }
        //        else
        //        {
        //            FO.WriteHeader(ws.SimulationRunResults, mine, sw);
        //        }
        //        stopHeader = true;

        //    }
        //    if (FO.WriteResults(ws.SimulationRunResults, ScnName, mine, sw))
        //    {
        //        //ScnCnt++;

        //    }
        //    else
        //    {
        //    }
        //    ws.Simulation_Stop();
        //}

        // ===========================================================================
        void runOnly()
        {
            for (int year = ws.Simulation_Start_Year; year < ws.Simulation_End_Year; ++year)
            {
                ws.Simulation_NextYear();
                StartSimulation = false;
            }
        }
    
        public class MyParameterInfo
        {
            protected KeyID FKey = new KeyID();

            public string Fieldname { get; set; }
            public int Code { get; set; }
            public string Name { get; set; }
            public int MinIntValue { get; set; }
            public int MaxIntValue { get; set; }
            public int RangeInterval { get; set; }
            public bool Done { get; set; }
            public ModelParameterClass TheMP { get; set; }
           // public ModelParameterClass MPC;
            public providerArrayProperty PAP { get; set; }
            //
            public MyParameterInfo() { }
            public MyParameterInfo(ModelParameterClass MP )
                : base()
            {
                TheMP = MP;

            }
            public MyParameterInfo(string fieldName, string name, int minintValue, int maxintValue, int rangeInterval, ModelParameterClass MP)
            {
                Fieldname = fieldName;
                Name = name;
                MinIntValue = minintValue;
                MaxIntValue = maxintValue;
                RangeInterval = rangeInterval;
                TheMP = MP;
            }

            public MyParameterInfo(string fieldName, string name, int minintValue, int maxintValue, int rangeInterval, ModelParameterClass MP, providerArrayProperty pap)
                
            {
                Fieldname = fieldName;             
                Name = name;
                MinIntValue = minintValue;
                MaxIntValue = maxintValue;
                RangeInterval = rangeInterval;
                PAP = pap;
                TheMP = MP;
            }
            // 
            public MyParameterInfo(string fieldName, int code, string name, int minintValue, int maxintValue, int rangeInterval, providerArrayProperty pap)
            {
                Fieldname = fieldName;
                Code = code;
                Name = name;
                MinIntValue = minintValue;
                MaxIntValue = maxintValue;
                RangeInterval = rangeInterval;
                //PAP = new providerArrayProperty();
                PAP = pap;
                //TheMP = mp;
            }

            //
            public void addParameter(string fieldName, string name, int minintValue, int maxintValue, int rangeInterval, ModelParameterClass MP)
            {
                //TheMP = new ModelParameterClass();
                Fieldname = fieldName;
                Name = name;
                MinIntValue = minintValue;
                MaxIntValue = maxintValue;
                RangeInterval = rangeInterval;
                TheMP = MP;
              }
            //
            public List<int[]> MyValuesList = new List<int[]>();         
            public void _recursive(string[] flds, int[] possiblevals, int[] scenvalues, int index)
            {
                int[] TempValues = scenvalues;
                int[] passValues = new int[scenvalues.Length];
                for (int i=0;i<scenvalues.Length;i++)
                {
                    passValues[i] = scenvalues[i];

                }
                //
                if (index == flds.Length)
                {
                    MyValuesList.Add(passValues);

                }
                else
                {
                    foreach (int FldInt in possiblevals)
                    {
                        TempValues[index] = FldInt;
                        _recursive(flds, possiblevals, TempValues, index + 1);


                    }
                   
                }

            }
            //
            public void cleanUp(List<int[]> aList)
            {
                for (int i = aList.Count - 1; i >= 0; i--)
                {
                    int[] Values = aList[i];
                    int total = 0;
                    foreach (int val in Values)
                    {
                        total += val;
                    }
                    if (aList.Count == 1331)
                    {
                        if (total != 100)
                        {
                            aList.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (total != 100)
                        {
                            if (total > 100)
                            {
                                aList.RemoveAt(i);
                            }
                            else
                            {
                                if (total < 60) 
                                {
                                    aList.RemoveAt(i);

                                }
                            }
                        }
                    }
                }

            }

            //
            public List<int[]> aValuesList = new List<int[]>();
            public void _Recursive(string[] flds, int[] possiblevals, int[] scenvalues, int index, List<int[]> aList)
            {
                int[] TempValues = scenvalues;
                int[] passValues = new int[scenvalues.Length];
                for (int i = 0; i < scenvalues.Length; i++)
                {
                    passValues[i] = scenvalues[i];
                }
                //
                if (index == flds.Length)
                {
                    aList.Add(passValues);
                    //
                    serveCleanUp(index, possiblevals, aList);
                    //if (index == 3)
                    //{
                    //    if (len == 11)
                    //    {
                    //        // Water Users (res, com, ind)
                    //        if (aList.Count == 1331)
                    //        {
                    //            cleanUp(aList);
                    //        }
                    //    }
                    //    if (len == 6)
                    //    {
                    //        // Reclaimed Water
                    //        if (aList.Count == 216)
                    //        {
                    //             cleanUp(aList);
                    //        }
                    //    }
                    //}
                    //else if (index == 2)
                    //{
                    //    // Effluent Water
                    //    if (aList.Count == 121)
                    //    {
                    //        cleanUp(aList);
                    //    }
                    //}
                }
                else
                {
                    foreach (int FldInt in possiblevals)
                    {
                        TempValues[index] = FldInt;
                        _Recursive(flds, possiblevals, TempValues, index + 1, aList);
                    }

                }

            }
            public void serveCleanUp(int index, int [] possible, List<int[]> aList)
            {
                int code = 0;
                int len = possible.Length;
                if (index == 3 && len == 11) code = 1;
                if (index == 3 && len == 6) code = 2;
                if (index == 2) code = 3;
                switch (code)
                {
                    case (1):
                        // Water Users (res, com, ind)
                        if (aList.Count == 1331)
                        {
                            cleanUp(aList);
                        }
                        break;
                    case (2):
                        // Effluent Water
                        if (aList.Count == 216)
                        {
                            cleanUp(aList);
                        }
                        break;
                    case (3):
                        if (aList.Count == 121)
                        {
                            cleanUp(aList);
                        }
                        break;
                }
            }
            public int[] Data = new int[33];
            public void SetData( int i)
            {
              
                switch (TheMP.ParamType)
                {
                    case modelParamtype.mptInputBase:
                        TheMP.Value = Data[0];
                        break;
                    case modelParamtype.mptInputProvider:
                        ProviderIntArray PData = new ProviderIntArray();               

                        PData.Values = Data;
                        TheMP.ProviderProperty.setvalues(PData);
                        break;
                }
            }
            

            public KeyID Key
            {
                get { return FKey; }
            }


        }
        public class MyParameterList
        {
            public List<MyParameterInfo> myList { get; set; }
            public MyParameterList()
            {
                myList = new List<MyParameterInfo>();
            }
            public void Add(MyParameterInfo parms)
            {
                myList.Add(parms);
            }
        }
        public class KeyID
        {
            static int count = 0;
            DateTime FDateTime;
            long FKey;
            //-----------------------
            /// <summary>   Default constructor. </summary>
            public KeyID()
            {
                FDateTime = DateTime.Now;

                FKey = FDateTime.ToBinary() + count;
                count++;
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Query if 'aKey' is equal to this keyID. </summary>
            ///
            /// <param name="aKey"> The key. </param>
            ///
            /// <returns>   true if equal, false if not. </returns>
            ///-------------------------------------------------------------------------------------------------

            public bool isEqual(KeyID aKey)
            {
                return (aKey.FKey == FKey);
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Calculates the hash code for this object. 
            ///             This is just the base Object hash code</summary>
            ///
            /// <returns>   The hash code for this object. </returns>
            ///
            /// <seealso cref="System.Object.GetHashCode()"/>
            ///-------------------------------------------------------------------------------------------------

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            ///-------------------------------------------------------------------------------------------------
            /// <summary>   Convert this object into a string representation.
            ///             Which is the Long int value of the KEYID, its uniqe code </summary>
            ///
            /// <returns>   A string representation of this object. </returns>
            ///
            /// <seealso cref="System.Object.ToString()"/>
            ///-------------------------------------------------------------------------------------------------

            public override string ToString()
            {
                return FKey.ToString();
            }
        }
          public void Externalsensitivity()
        {
            ScnGenForm();
            pl.Add(new MyParameterInfo("PCWSRES", "Water Supply To Res", 0, 100, 10, WSRES));
            pl.Add(new MyParameterInfo("PCWSCOM", "Water Supply To Com", 0, 100, 10, WSCOM));
            pl.Add(new MyParameterInfo("PCWSIND", "Water Supply To Ind", 0, 100, 10, WSIND));

            pl.Add(new MyParameterInfo("PCPUMPDFLT", "DefaultPumping", 1, 5, 1, SdefaultPump, ws.Default_Pumping_MnI_PCT));
            pl.Add(new MyParameterInfo("WEBPOPGR", "Web Pop Growth Rate", 0, 100, 20, WebPopGrRate));

            pl.Add(new MyParameterInfo("GPCDMIN",  "Minimum GPCD", 50, 90, 10, GPCDmin));
            pl.Add(new MyParameterInfo("PCGRTON",  "PopGrowthRate On-project", 0, 150, 25, GrRateOnProject));
            pl.Add(new MyParameterInfo("PCGRTOFF", "PopGrowthRate Other", 0, 150, 25, GrRateOffProject));
            //
            //
             pl.Add(new MyParameterInfo("PROUTUSE", "Res Outdoor Water Use", 10, 90, 10, RESOUT));
            pl.Add(new MyParameterInfo("PCOUTUSE", "Com Outdoor Water Use", 10, 90, 10, COMOUT));
            pl.Add(new MyParameterInfo("PIOUTUSE", "Ind Outdoor Water Use", 10, 90, 10, INDOUT));
            //
            pl.Add(new MyParameterInfo("PCRECDI", "Reclaimed to DI", 0, 100, 20, RECtoDI));
            pl.Add(new MyParameterInfo("PCERECWS", "ReclaimedToOutput", 0, 100, 20, RECtoWS));
            pl.Add(new MyParameterInfo("PCRECVAD", "Reclaimed to Vadose pct", 0, 100, 20, RECtoVadose));
              //
            pl.Add(new MyParameterInfo("PCDEMREC", "ReclaimedToWSmax", 0, 70, 10, MaxREC));
            pl.Add(new MyParameterInfo("PCRECOUT", "ReclaimedUsedOutdoors", 0, 100, 10, RECoutdoors));
            pl.Add(new MyParameterInfo("PCRECRO",  "ReclaimedPushedtoRO", 0, 100, 10, RtoRO));
            pl.Add(new MyParameterInfo("PCROWS",   "ROToWaterSupply", 0, 100, 10, ROtoWS));
            pl.Add(new MyParameterInfo("PCWWEFF",  "WasteWaterToEffluent", 0, 100, 10, WWtoEff));
            pl.Add(new MyParameterInfo("PCEFFREC", "WasteWaterToReclaimedWWTP", 0, 100, 10, WWtoReclaimed));
            pl.Add(new MyParameterInfo("PCEFFVAD", "EffluentToVadose", 0, 100, 10, EffToVadose));
            pl.Add(new MyParameterInfo("PCEFFPP",  "EffluentToPowerPlant", 0, 100, 10, EffToPower));
            pl.Add(new MyParameterInfo("WEBAUGPCT","Augumented Water pct", 0, 25, 5, WAugParm));
            //
            pl.Add(new MyParameterInfo("AWSLIMIT", "Assured Water Supply Switch", 0, 1, 1,  AWSlimit));
            pl.Add(new MyParameterInfo("VADLAG",   "Time Lag Vadose", 5, 45, 5, TimeLag));
            pl.Add(new MyParameterInfo("MNFLOW",   "Normal Flow Rights", 100, 550, 50, ModNormal));
            pl.Add(new MyParameterInfo("PCSWVAD",  "SurfaceWaterToVadose", 0, 100000, 10000, SurfToVadose));
            pl.Add(new MyParameterInfo("PCSWWB",   "SurfaceWaterToWBpct", 0, 100, 10, SurfToWBpct));
            pl.Add(new MyParameterInfo("USEWSDI",  "WaterSupplyToDI", 0, 100000, 10000, WStoDI));

            //
            pl.Add(new MyParameterInfo("SVEXTSTYR", "Trace Year Start SVT", 1946, 1979, 2, STVtrace));
            pl.Add(new MyParameterInfo("SVCLMADJ", "Flow Modification SVT", 20, 120, 20, STVClimate));
            pl.Add(new MyParameterInfo("COEXTSTYR", "Trace Year Start CO", 1906, 1979, 2, COTrace));
            pl.Add(new MyParameterInfo("COCLMADJ", "Climate on Flows CO", 20, 120, 20, COClimate));
            pl.Add(new MyParameterInfo("WATAUG", "Water Augmentation", 0, 20, 4, WAugParm));
         }
          ModelParameterClass TempParam;
          internal bool parameterCheck(MyParameterInfo MP, int i)
        {
           TempParam = ws.ParamManager.Model_Parameter(MP.TheMP.ModelParam);
            ModelParameterClass TheMP = ws.ParamManager.Model_Parameter(MP.Fieldname);
           
            bool result = true;
            int[] Data = new int[33];
            for (int j = 0; j < 33; j++)
            {
                Data[j] = i;
            }
            switch (TheMP.ParamType)
            {
                case modelParamtype.mptInputBase:
                    //TheMP.Value = Data[0];
                    TempParam.Value = Data[0];
                    break;
                case modelParamtype.mptInputProvider:
                    ProviderIntArray PData = new ProviderIntArray();
                    PData.Values = Data;
                    TheMP.ProviderProperty.setvalues(PData);
                    break;
            }
                    
                // ws.ParamManager.Model_Parameter("DROUSCEN").Value = 3;
            int data = i;
            for (int j = 0; j < 33; j++) { MP.Data[j] = i; }

            return result;

        }
        //
          internal bool parameterSet(ModelParameterBaseClass MP, MyParameterInfo MPC, int i)
          {
              ModelParameterClass TheMP = ws.ParamManager.Model_Parameter(MP.Fieldname);

              bool result = true;
              int[] Data = new int[33];
              for (int j = 0; j < 33; j++)
              {
                  Data[j] = i;
              }
              switch (TheMP.ParamType)
              {
                  case modelParamtype.mptInputBase:
                      //TheMP.Value = Data[0];
                      TempParam.Value = Data[0];
                      break;
                  case modelParamtype.mptInputProvider:
                      ProviderIntArray PData = new ProviderIntArray();
                      PData.Values = Data;
                      TheMP.ProviderProperty.setvalues(PData);
                      break;
              }

              // ws.ParamManager.Model_Parameter("DROUSCEN").Value = 3;
              int data = i;
              for (int j = 0; j < 33; j++) { MPC.Data[j] = i; }

              return result;

          }

        //
          internal bool parameterZeroOut(ModelParameterBaseClass MP, MyParameterInfo MPC)
          {        
              ModelParameterClass TheMP = ws.ParamManager.Model_Parameter(MP.Fieldname);
              bool result = true;
              int[] Data = new int[33];
              for (int j = 0; j < 33; j++)
              {
                  Data[j] = 0;
              }
              switch (TheMP.ParamType)
              {
                  case modelParamtype.mptInputBase:
                      TempParam.Value = Data[0];
                      break;
                  case modelParamtype.mptInputProvider:
                      ProviderIntArray PData = new ProviderIntArray();
                      PData.Values = Data;
                      TheMP.ProviderProperty.setvalues(PData);
                      break;
              }

              int data = 0;
              for (int j = 0; j < 33; j++) { MPC.Data[j] = 0; }
              return result;

          }

        //
        public void ScnGenForm()
        {
            ws.IncludeAggregates = true;
            SdefaultPump = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Default_MnI_Pumping); //Default_Pumping_MnI_PCT
            set_upperBasinData = 2;
            //
            WebPopGrRate = ws.ParamManager.Model_Parameter(eModelParam.epWebPop_GrowthRateAdj_PCT);
            GrRateOnProject = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Growth_Rate_Adjustment_OnProject);
            GrRateOffProject = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Growth_Rate_Adjustment_Other);
            WebPrUse = ws.ParamManager.Model_Parameter(eModelParam.epWebUIPersonal_PCT);
           // PopOnProject = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Growth_Rate_Adjustment_OnProject);
           // PopOffProject = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Growth_Rate_Adjustment_Other);
            //
            WebGPCD = ws.ParamManager.Model_Parameter(eModelParam.epWebUIPersonal_PCT);
            GPCDmin = ws.ParamManager.Model_Parameter(eModelParam.epProvider_GPCDmin);
            ///
            STVtrace = ws.ParamManager.Model_Parameter(eModelParam.epSaltVerde_Historical_Extraction_Start_Year);
            STVClimate = ws.ParamManager.Model_Parameter(eModelParam.epSaltVerde_Climate_Adjustment_Percent);
            STVDroughtStart = ws.ParamManager.Model_Parameter(eModelParam.epColorado_User_Adjustment_StartYear);
            STVDroughtStop = ws.ParamManager.Model_Parameter(eModelParam.epColorado_User_Adjustment_Stop_Year);
            STVDroughtPCT = ws.ParamManager.Model_Parameter(eModelParam.epSaltVerde_User_Adjustment_Percent);
            //
            COTrace = ws.ParamManager.Model_Parameter(eModelParam.epColorado_Historical_Extraction_Start_Year);
            COClimate = ws.ParamManager.Model_Parameter(eModelParam.epColorado_Climate_Adjustment_Percent);
            CODroughtStart = ws.ParamManager.Model_Parameter(eModelParam.epColorado_User_Adjustment_StartYear);
            CODroughtStop = ws.ParamManager.Model_Parameter(eModelParam.epColorado_User_Adjustment_Stop_Year);
            CODroughtPCT = ws.ParamManager.Model_Parameter(eModelParam.epColorado_User_Adjustment_Percent);
            //  years[i] = DepthToGroundwater[i] * 0.075;
            VadoseLag = ws.ParamManager.Model_Parameter(eModelParam.epSurface_to_Vadose_Time_Lag);
            RECtoVadose = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Reclaimed_to_Vadose);
            RECtoDI = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Reclaimed_to_DirectInject);
            RECtoWS = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Reclaimed_to_Water_Supply);
            MaxREC = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Max_Demand_Reclaim);
            RECoutdoors = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Reclaimed_Outdoor_Use);
            RtoRO = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Reclaimed_to_RO);
            ROtoWS = ws.ParamManager.Model_Parameter(eModelParam.epPCT_RO_to_Water_Supply);
            //
            EffToVadose = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Effluent_to_Vadose);
            EffToPower = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Effluent_to_PowerPlant);
            //
            WSRES = ws.ParamManager.Model_Parameter(eModelParam.epPCT_WaterSupply_to_Residential);
            WSCOM = ws.ParamManager.Model_Parameter(eModelParam.epPCT_WaterSupply_to_Commercial);
            WSIND = ws.ParamManager.Model_Parameter(eModelParam.epPCT_WaterSupply_to_Industrial);
            //
            RESOUT = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Outdoor_WaterUseRes);
            COMOUT = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Outdoor_WaterUseCom);
            INDOUT = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Outdoor_WaterUseInd);
            //
            TimeLag = ws.ParamManager.Model_Parameter(eModelParam.epSurface_to_Vadose_Time_Lag);
            ModNormal = ws.ParamManager.Model_Parameter(eModelParam.epModfyNormalFlow);
            SurfToVadose = ws.ParamManager.Model_Parameter(eModelParam.epSurfaceWater__to_Vadose);
            SurfToWBpct = ws.ParamManager.Model_Parameter(eModelParam.epPCT_SurfaceWater_to_WaterBank);
            WStoDI = ws.ParamManager.Model_Parameter(eModelParam.epUse_WaterSupply_to_DirectInject);
            //
            WWtoReclaimed = ws.ParamManager.Model_Parameter(eModelParam.epPCT_WasteWater_to_Reclaimed);

            WAugParm = ws.ParamManager.Model_Parameter(eModelParam.epWebAugmentation_PCT);
            WWtoEff = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Wastewater_to_Effluent);

            //// Regional parameterizations - unusual parameters
            AWSlimit = ws.ParamManager.Model_Parameter(eModelParam.epAWSAnnualGWLimit);

            //
        }
       

    }
    //
    //public class ScenarioData
    //{
    //    List<ParmData> ParmDatas = new List<ParmData>();
    //    string FScnName = "";
    //    public ScenarioData(string aName)
    //    {
    //        FScnName = aName;
    //    }

    //    public void AddParmData(ParmData PD)
    //    {
    //        ParmDatas.Add(PD);
    //    }

    //    public List<ParmData> Data
    //    {
    //        get { return ParmDatas; }
    //    }
    //    public string Name
    //    {
    //        get { return FScnName; }
    //    }
    //}
    //public class ParmData
    //{
    //    string Fieldname = "";
    //    int[] Data = new int[33];
    //    ModelParameterClass TheMP;
    //    public ParmData(string Field, int[] PData, ParameterManagerClass PM)
    //    {
    //        Data = PData;
    //        Fieldname = Field;
    //        TheMP = PM.Model_Parameter(Field);
    //    }

    //    public void SetData()
    //    {
    //        switch (TheMP.ParamType)
    //        {
    //            case modelParamtype.mptInputBase:
    //                TheMP.Value = Data[0];
    //                break;
    //            case modelParamtype.mptInputProvider:
    //                ProviderIntArray PData = new ProviderIntArray();
    //                PData.Values = Data;
    //                TheMP.ProviderProperty.setvalues(PData);
    //                break;
    //        }
    //    }
    //}

}
