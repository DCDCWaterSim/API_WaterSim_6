using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StreamOutputs;
using WaterSimDCDC;
using WaterSim;
namespace API_WaterSim
{
    public class myVariables : WaterSimU
    {
        internal FileOutputsBase FOB;
        //
        ModelParameterClass WAugParm;
        ModelParameterClass WWtoEff;
        ModelParameterClass WWtoReclaimed;
        ModelParameterClass RtoRO;
        //
        ProviderIntArray Out = new ProviderIntArray(0);
        internal int[] OneHundred = new int[ProviderClass.NumberOfProviders];
        internal int[] Ninty = new int[ProviderClass.NumberOfProviders];
        internal int[] Fifty = new int[ProviderClass.NumberOfProviders];
        internal int[] Ten = new int[ProviderClass.NumberOfProviders];
        internal int[] Zero = new int[ProviderClass.NumberOfProviders];

        // Set the parameters used for output to the text file
        public void myParameters()
        {
            FOB = new FileOutputsBase();
            FOB.myParms.Add(eModelParam.epSaltVerde_Historical_Extraction_Start_Year);
            FOB.myParms.Add(eModelParam.epColorado_Historical_Extraction_Start_Year);
            FOB.myParms.Add(eModelParam.epColorado_River_Flow);
            FOB.myParms.Add(eModelParam.epSaltTonto_AnnualFlow);
            FOB.myParms.Add(eModelParam.epVerde_AnnualFlow);
            //
            FOB.myParms.Add(eModelParam.epRainFallFactor);
            FOB.myParms.Add(eModelParam.epProvider_IwaniecScenario);
            FOB.myParms.Add(eModelParam.epSaltVerde_Annual_Deliveries_SRP);
            FOB.myParms.Add(eModelParam.epColorado_Annual_Deliveries);
            FOB.myParms.Add(eModelParam.epGroundwater_Bank_Used);
            FOB.myParms.Add(eModelParam.epGroundwater_Bank_Balance);
            FOB.myParms.Add(eModelParam.epWaterAugmentationUsed);
            FOB.myParms.Add(eModelParam.epGroundwater_Pumped_Municipal);
            FOB.myParms.Add(eModelParam.epReclaimed_Water_Used);
            FOB.myParms.Add(eModelParam.epProvider_Nonpotable);
            FOB.myParms.Add(eModelParam.epRainHarvestedToTotalOutdoor);
            FOB.myParms.Add(eModelParam.epGrayWaterToTotalOutdoor);
            FOB.myParms.Add(eModelParam.epReclaimedToTotalOutdoor);
            FOB.myParms.Add(eModelParam.epProvider_RainWaterHarvestedSF);
            FOB.myParms.Add(eModelParam.epProvider_RainWaterHarvestedMF);
            FOB.myParms.Add(eModelParam.epProvider_RainWaterHarvestedPU);
            FOB.myParms.Add(eModelParam.epProvider_ResGrayWaterUsed);
            FOB.myParms.Add(eModelParam.epProvider_ComGrayWaterUsed);
            FOB.myParms.Add(eModelParam.epProvider_IndGrayWaterUsed);
            FOB.myParms.Add(eModelParam.epProvider_RainWaterHarvested);
            //
            FOB.myParms.Add(eModelParam.epProvider_ResIndoorGPCD_LD);
            FOB.myParms.Add(eModelParam.epProvider_ResIndoorGPCD_MD);
            FOB.myParms.Add(eModelParam.epProvider_ResIndoorGPCD_HD);
            FOB.myParms.Add(eModelParam.epProvider_ResOutdoorGPCD_LD);
            FOB.myParms.Add(eModelParam.epProvider_ResOutdoorGPCD_MD);
            FOB.myParms.Add(eModelParam.epProvider_ResOutdoorGPCD_HD);
            //
            // FOB.myParms.Add(eModelParam.epDemand_Agriculture);
            FOB.myParms.Add(eModelParam.epDemand_LowDensity);
            FOB.myParms.Add(eModelParam.epDemand_MediumDensity);
            FOB.myParms.Add(eModelParam.epDemand_HighDensity);
             FOB.myParms.Add(eModelParam.epDemand_Turf);
             FOB.myParms.Add(eModelParam.epDemand_Greenway);
             FOB.myParms.Add(eModelParam.epDemand_Tree);
            FOB.myParms.Add(eModelParam.epResidentialIndoorWaterUse);
            FOB.myParms.Add(eModelParam.epResidentialOutdoorWaterUse);
            FOB.myParms.Add(eModelParam.epRegionalGWBalance);
            //
            FOB.myParms.Add(eModelParam.epProvider_StormWaterHarvested);
            //
            //
            FOB.eModelParametersForOutput = new int[FOB.myParms.Count];
            for (int i = 0; i < FOB.myParms.Count; i++)
            {
                FOB.eModelParametersForOutput[i] = FOB.myParms[i];
            }
        }
        public void Initialize(int Scenario,WaterSimManager_SIO ws)
        {
            //int gpcdReduce = 100;
            //int reuse = 0;
            // int reuse=100;
            //
            ws.Web_PopulationGrowthRate_PCT = 100;
            set_parmIncludeMeteorology = true;
            //parmAPIcleared = true;
            parmIwaniecScenariosYN = true;
            // ================================================================================
            WAugParm = ws.ParamManager.Model_Parameter(eModelParam.epWebAugmentation_PCT);
            WWtoEff = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Wastewater_to_Effluent);
            WWtoReclaimed = ws.ParamManager.Model_Parameter(eModelParam.epPCT_WasteWater_to_Reclaimed);
            RtoRO = ws.ParamManager.Model_Parameter(eModelParam.epPCT_Reclaimed_to_RO);
            //
            // 01.11.17 based on conversations with Elizabeth
            // ==============================================
            for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 50; }
            Out.Values = OneHundred;
            ws.PCT_Reclaimed_Outdoor_Use.setvalues(Out);
            ws.PCT_Wastewater_Reclaimed.setvalues(Out);
            // ==============================================
            //


            // ========================================
            // RCP8.5 emission scenario
            //CNRM.a2 GCM with a2 emission
            // 60 for the Colorado River
            // 55 (estimate) for the Salt-Verde-Tonto Rivers
            ws.Colorado_Climate_Adjustment_Percent = 60;
            ws.SaltVerde_Climate_Adjustment_Percent = 55;
            ws.Simulation_End_Year = 2060;
            //
            // 1=AD; 2 = AF; 3=AH; 4=HHH; 5=EC; 6=ZW; 7=BAU; 8=testing; 9=default
            // int scenario = 7;
            // int scenario = Scenario;
            //set_scenario = Scenario;
            // Default Values
            for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 0; }
            Out.Values = OneHundred;
            ws.PCT_SurfaceWater_to_WaterBank.setvalues(Out);
            ws.WaterAugmentation.setvalues(Out);
            ws.RainFallFactor = 100;
            // ===============================================
            //
            string path = @"BAU\";
            switch (Scenario)
            {
                case 1:
                    path = @"AD\";
                    //
                    // Capture Rain water Residential
                    set_parmCalculateRainWaterHarvesting = true;
                    // Capture Rain water Commercial and Industrial too
                    set_parmCalculateRainWaterResidentialOnly = false;
                    // Capture Storm Water - default = 50% of maximum 60-year values
                    set_parmCalculateStormWaterHarvesting = true;
                    // NOTE: Using a value > 0 for the Structural Capacity overrides the default 50% of rainfall maximum
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Zero[i] = 0; }
                    Out.Values = Zero;
                    ws.StormWaterStructural_Capacity.setvalues(Out);
                    //
                    // Utilize Gray water
                    set_parmCalculateGrayWater = true;
                    // 90% of the houses implement Gray Water Recycling
                    set_parmGrayWaterCompliancePCT = 90;
                    //
                    // Bank excess CAP surface Water
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 100; }
                    Out.Values = OneHundred;
                    ws.PCT_SurfaceWater_to_WaterBank.setvalues(Out);
                    //
                    set_parmIwaniecNoLeaks = true;
                    // Linearly reduce effluent goint to Paleo Verde over time.
                    set_parmIwaniecPPtoAgEffluent = true;
                    // Use Augmented Water - 10% of demand
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Ten[i] = 10; }
                    Out.Values = Ten;
                    ws.WaterAugmentation.setvalues(Out);
                    break;
                case 2:
                    path = @"AF\";

                    // Adaptive flood
                    set_parmCalculateRainWaterHarvesting = true;
                    set_parmCalculateRainWaterResidentialOnly = false;
                    //
                    set_parmCalculateStormWaterHarvesting = true;
                    set_parmCalculateStormWaterHarvesting = false;
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 1000; }
                    Out.Values = OneHundred;
                    ws.StormWaterStructural_Capacity.setvalues(Out);
                    //
                    set_parmCalculateGrayWater = true;
                    //set_parmGrayWaterCompliancePCT = 77;
                    //set_parmYearsToAdopt = 15;
                    set_parmIwaniecNoLeaks = false;
                    set_parmIwaniecPPtoAgEffluent = false;
                    WAugParm.Value = 0;
                    break;
                case 3:
                    path = @"AH\";

                    // Adaptive Heat
                    set_parmCalculateRainWaterHarvesting = true;
                    set_parmCalculateRainWaterResidentialOnly = false;
                    set_parmCalculateStormWaterHarvesting = false;
                    set_parmCalculateGrayWater = true;
                    set_parmGrayWaterCompliancePCT = 95;
                    set_parmYearsToAdopt = 15;
                    set_parmIwaniecNoLeaks = false;
                    set_parmIwaniecPPtoAgEffluent = false;
                    WAugParm.Value = 0;
                    break;
                case 4:
                    path = @"HHH\";

                    // Transformative Healthy Harvest Hubs
                    set_parmCalculateRainWaterHarvesting = false;
                    set_parmCalculateRainWaterResidentialOnly = true;
                    set_parmCalculateStormWaterHarvesting = true;
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Zero[i] = 0; }
                    Out.Values = Zero;
                    // Using the Structural Capacity overrides 
                    ws.StormWaterStructural_Capacity.setvalues(Out);

                    set_parmCalculateGrayWater = true;
                    set_parmGrayWaterCompliancePCT = 100;
                    set_parmYearsToAdopt = 15;
                    set_parmIwaniecNoLeaks = false;
                    set_parmIwaniecPPtoAgEffluent = true;
                    WAugParm.Value = 0;

                    break;
                case 5:
                    path = @"EC\";

                    // Transformative Emerald City Necklace
                    set_parmCalculateRainWaterHarvesting = true;
                    set_parmCalculateRainWaterResidentialOnly = false;
                    set_parmCalculateStormWaterHarvesting = true;
                    // Using the Structural Capacity overrides 
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 1000; }
                    Out.Values = OneHundred;
                    ws.StormWaterStructural_Capacity.setvalues(Out);

                    set_parmCalculateGrayWater = true;
                    set_parmGrayWaterCompliancePCT = 77;
                    set_parmYearsToAdopt = 15;
                    set_parmIwaniecNoLeaks = false;
                    //
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Zero[i] = 0; }
                    Out.Values = Zero;
                    ws.PCT_Effluent_to_Vadose.setvalues(Out);
                    //
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Ninty[i] = 80; }
                    Out.Values = Ninty;
                    ws.PCT_Wastewater_to_Effluent.setvalues(Out);
                    //
                    set_parmIwaniecPPtoAgEffluent = true;
                    WAugParm.Value = 0;

                    break;
                case 6:
                    path = @"ZW\";

                    // Transformative Zero Waste
                    set_parmCalculateRainWaterHarvesting = true;
                    set_parmCalculateRainWaterResidentialOnly = false;
                    //set_parmCalculateStormWaterHarvesting = true;
                    set_parmCalculateStormWaterHarvesting = false;
                    // Using the Structural Capacity overrides
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Zero[i] = 0; }
                    Out.Values = Zero;
                    ws.StormWaterStructural_Capacity.setvalues(Out);

                    set_parmCalculateGrayWater = true; // change from initial
                    //set_parmGrayWaterCompliancePCT = 100;
                    //set_parmYearsToAdopt = 15;
                    set_parmIwaniecNoLeaks = true;
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Zero[i] = 0; }
                    Out.Values = Zero;
                    ws.PCT_Effluent_to_Vadose.setvalues(Out);
                    //
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { Fifty[i] = 50; }
                    Out.Values = Fifty;
                    ws.PCT_Wastewater_to_Effluent.setvalues(Out);

                    set_parmIwaniecPPtoAgEffluent = true;
                    WAugParm.Value = 0;

                    break;
                case 7:
                    path = @"BAU\";
                    // Testing
                        set_parmCalculateRainWaterHarvesting = true;
                        set_parmCalculateRainWaterResidentialOnly = false;
                        set_parmCalculateRainWaterCommercialOnly = false;
                        //
                        //set_parmCalculateStormWaterHarvesting = true;
                        set_parmCalculateStormWaterHarvesting = false;
                        for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 100; }
                        Out.Values = OneHundred;
                        ws.StormWaterStructural_Capacity.setvalues(Out);
                        //
                        set_parmCalculateGrayWater = true;
                        set_parmIwaniecNoLeaks = false;
                        // WWtoReclaimed.Value = 0;
                        // WWtoEff.Value = 100;
                        set_parmIwaniecPPtoAgEffluent = false;
                     break;
                case 8:
                    path = @"BAU\";
                    // Testing
                    set_parmCalculateRainWaterHarvesting = false;
                    set_parmCalculateRainWaterResidentialOnly = false;
                    set_parmCalculateRainWaterCommercialOnly = false;
                    set_parmCalculateStormWaterHarvesting = false;
                    //
                    for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 1; }
                    Out.Values = OneHundred;
                    ws.StormWaterStructural_Capacity.setvalues(Out);

                    set_parmCalculateGrayWater = false;
                    //set_parmGrayWaterCompliancePCT = 30;
                    //set_parmYearsToAdopt = 15;
                    set_parmIwaniecNoLeaks = false;
                    // WWtoReclaimed.Value = 0;
                    // WWtoEff.Value = 100;
                    set_parmIwaniecPPtoAgEffluent = false;

                    break;
                case 9:

                    break;
                default:
                    path = @"BAU\";

                    set_parmIncludeMeteorology = true;
                    set_parmCalculateRainWaterHarvesting = false;
                    set_parmCalculateStormWaterHarvesting = false;

                    break;
            }
            //
            ws.IwaniecScenarios = Scenario;
            //
            // Copy the appropriate LCLU classification file into the Data Directory prior to the simulation
            // 07.05.16
            // SimpleFileCopy myFile = new SimpleFileCopy();
            //myFile.CopyMain(path);
            //
            //InitSecondary(gpcdReduce, reuse);
            // ws.Simulation_End_Year = 2060;
            // -------------------------------------
            bool testing = false;
            if (testing)
            {
                // General Testing of NEW array structure
                for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 1000; }
                Out.Values = OneHundred;
                //ws.SurfaceWater__to_Vadose.setvalues(Out);
                for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 50; }
                Out.Values = OneHundred;
                //
                for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 1000; }
                Out.Values = OneHundred;
                //ws.Use_WaterSupply_to_DirectInject.setvalues(Out);
                //
                for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 50; }
                Out.Values = OneHundred;
                //ws.PCT_Wastewater_Reclaimed.setvalues(Out);
                //
                for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 50; }
                Out.Values = OneHundred;
                //ws.PCT_Reclaimed_to_RO.setvalues(Out);
                //
                for (int i = 0; i < ProviderClass.NumberOfProviders; i++) { OneHundred[i] = 100; }
                Out.Values = OneHundred;
                //ws.PCT_RO_to_Water_Supply.setvalues(Out);
                //
            }
            // ===================================================================================

        }

    }


}
