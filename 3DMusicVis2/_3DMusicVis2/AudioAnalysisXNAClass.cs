//
//Audio Analysis Framework - Created by Stephen Hoult.
//Downloaded from: http://www.codeproject.com/Articles/593627/An-Audio-Analysis-Framework-for-XNA-Developers
//Contact: Through the linked articles comments and discussions. 
//Please do not remove this :).
//

//Required includes:
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

/////////////////////////////////////////////////////
//Change the name space to your projects namespace://
/////////////////////////////////////////////////////
namespace _3DMusicVis2 //<---------- Change this to your projects namespace. 
{
    //This class can be used for the real-time analysis of audio files that are currently being played by XNA's 'MediaPlayer' class.
    public class AudioAnalysisXNAClass //public to allow for unit testing.
    {

        //This is where the audio Frequencies/Amplitude and Sample Wave data is stored.
        //Frequencies are an array of 256 amplitude float values from 0f to 1f
        //Samples are an array of 256 wave float values from -1f to 1f
        VisualizationData visData = new VisualizationData();

        //Variables for storing averages of low, medium, and high freqency band amplitudes. 
        private float lowFrBandAverage;        
        private float midFrBandAverage;
        private float highFrBandAverage;

        //Constructor to switch visulization data on:
        public AudioAnalysisXNAClass()
        {
            //Enable visualization data
            MediaPlayer.IsVisualizationEnabled = true;
        }
        
        //Getters and Setters (Mainly for testing) - Though these can also be used by the user to create their own extra functions.
        public float LowFrBandAverage
        {
            get { return lowFrBandAverage; }
            set { lowFrBandAverage = value; }
        }
        public float MidFrBandAverage
        {
            get { return midFrBandAverage; }
            set { midFrBandAverage = value; }
        }
        public float HighFrBandAverage
        {
            get { return highFrBandAverage; }
            set { highFrBandAverage = value; }
        }
        
        //***********************************************************************************************************************
        //START OF SECTION: This section aquires and prepares the sample data for use. It's advised that these methods are to  **
        //be used only once, in the games update method - to minimize performance issues and maximise consisistency per frame. **
        //***********************************************************************************************************************
        //Collects visulisation data every update.
        public void Update() //This MUST be called first - Before any other updates 
        {
            //Set the new Visualization data every update
            MediaPlayer.GetVisualizationData(visData);
        }

        //Low frequency band update to identify rhythm. 
        public void updateAverageLowFrequencyData()
        {
            //Low: 20 – 600 Hz (See assignment for details on the frequency bands i've chosen).
            //10^(0 * 0.01171875 + 1.30103) = 20.00 Hz
            //10^(126 * 0.01171875 + 1.30103) = 599.22 Hz
            //So I will take an average of the amplitudes contained in frequencies 0 to 126 the visData. (127 samples total)
            float lowFrqData = 0;
            for (int i = 0; i < 127; i++)
            {
                lowFrqData += visData.Frequencies[i];
            }
            lowFrBandAverage = lowFrqData / 127;
        }

        //Mid range frequency band update to identify vocal samples and instrument harmonics 
        public void updateAverageMidFrequencyData()
        {
            //Mid: 600 Hz – 2.4 kHz  (See assignment for details on the frequency bands i've chosen).
            //10^(127 * 0.01171875 + 1.30103) = 615.61 Hz
            //10^(178 * 0.01171875 + 1.30103) = 2437.62 Hz
            //So I will take an average of the amplitudes contained in frequencies 127 - 178 from the visData. (52 samples total).
            float midFrqData = 0;
            for (int i = 127; i < 179; i++)
            {
                midFrqData += visData.Frequencies[i];
            }
            midFrBandAverage = midFrqData / 52;
        }

        //High range frequency band update to identify life and brightness of the audio.
        public void updateAverageHighFrequencyData()
        {
            //High: 2.4 kHz – 20 kHz  (See assignment for details on the frequency bands i've chosen).
            //10^(179 * 0.01171875 + 1.30103) = 2504.29 Hz
            //10^(255 * 0.01171875 + 1.30103) = 19467.54 Hz
            //So I will take an average of the amplitudes contained in frequencies 179 to 255 the visData. (77 samples total)
            //NOTE: Quick math check: 127+52+77 = 256. This is the correct total number of frequencies (0 - 255). 
            float highFrqData = 0;
            for (int i = 179; i < 256; i++)
            {
                highFrqData += visData.Frequencies[i];
            }
            highFrBandAverage = highFrqData / 77;
        }
        //***********************************************************************************************************************
        //END OF SECTION: Aquires and prepare the sample data for use. **********************************************************
        //***********************************************************************************************************************

        //***********************************************************************************************************************
        /*Game Mechanic Functions Below: Use these functions to aquire dynamic variables for game mechanics such as movement ****
         * speed, fire rate TimeSpan, particle effect velocities, spawn time TimeSpan, colours, colour combinations, ************
         * animation scale, etc. These methods provide return type for bool, int, float, String, Color, Color[] vector2s, *******
         * TimeSpan *///*********************************************************************************************************
        //***********************************************************************************************************************
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 1: 2 Step Return Bool----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Using the low frequency band average.
        public bool getBool_2StepLowFrq(float min) //return true if the min value is exceeded
        {
            if (lowFrBandAverage > min) 
                return true;

            return false;
        }

        //Using the mid frequency band average.
        public bool getBool_2StepMidFrq(float min) //return true if the min value is exceeded
        {
            if (midFrBandAverage > min)
                return true;

            return false;
        }

        //Using the high frequency band average.
        public bool getBool_2StepHighFrq(float min) //return true if the min value is exceeded
        {
            if (highFrBandAverage > min)
                return true;

            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 2: 2 Step Return Int----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Return the maxInt value if the min value is exceeded, otherwise return the minInt value.
        //Using the low frequency band average.
        public int getInt_2StepLowFrq(int minInt, int maxInt, float min)
        {
            if (lowFrBandAverage > min)
                return maxInt;

            return minInt;
        }

        //Using the mid frequency band average.
        public int getInt_2StepMidFrq(int minInt, int maxInt, float min)
        {
            if (midFrBandAverage > min)
                return maxInt;

            return minInt;
        }

        //Using the high frequency band average.
        public int getInt_2StepHighFrq(int minInt, int maxInt, float min)
        {
            if (highFrBandAverage > min)
                return maxInt;

            return minInt;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 3: 6 Step Return Int Float----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Return the minInt value if the min value is not exceeded by the lowFrBandAverage.
        //Return minInt + increment if min < lowFrBandAverage <= x1. 
        //Return minInt + increment * 2 if x1 < lowFrBandAverage <= x2. 
        //Return minInt + increment * 3 if x2 < lowFrBandAverage <= x3. 
        //Return minInt + increment * 4 if x3 < lowFrBandAverage <= max. 
        //Return minInt + increment * 5 if lowFrBandAverage > max.
        //Using the low frequency band average.
        public int getInt_6StepLowFrq(int minInt, int maxInt, float min, float x1, float x2, float x3, float max)
        {
            int range = maxInt - minInt; //Find the range
            float increment = range / 5; //Divide the range by 5, to create an incremental step value to be used below:

            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                return (int)(minInt + increment); //minimun movement speed + 1/5th of the range

            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                return (int)(minInt + increment * 2); //minimun movement speed + 2/5th of the range

            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                return (int)(minInt + increment * 3); //minimun movement speed + 3/5th of the range

            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                return (int)(minInt + increment * 4); //minimun movement speed + 4/5th of the range

            else if (lowFrBandAverage > max)
                return maxInt; ////minimun movement speed + 5/5th of the range = Equal to maximun movement speed :)

            return minInt; //minimun movement speed
        }

        //Using the mid frequency band average.
        public int getInt_6StepMidFrq(int minInt, int maxInt, float min, float x1, float x2, float x3, float max)
        {
            int range = maxInt - minInt; //Find the range
            float increment = range / 5; //Divide the range by 5, to create an incremental step value to be used below:

            if (midFrBandAverage > min && midFrBandAverage <= x1)
                return (int)(minInt + increment); //minimun movement speed + 1/5th of the range

            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                return (int)(minInt + increment * 2); //minimun movement speed + 2/5th of the range

            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                return (int)(minInt + increment * 3); //minimun movement speed + 3/5th of the range

            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                return (int)(minInt + increment * 4); //minimun movement speed + 4/5th of the range

            else if (midFrBandAverage > max)
                return maxInt; ////minimun movement speed + 5/5th of the range = Equal to maximun movement speed :)

            return minInt; //minimun movement speed
        }

        //Using the high frequency band average.
        public int getInt_6StepHighFrq(int minInt, int maxInt, float min, float x1, float x2, float x3, float max)
        {
            int range = maxInt - minInt; //Find the range
            float increment = range / 5; //Divide the range by 5, to create an incremental step value to be used below:

            if (highFrBandAverage > min && highFrBandAverage <= x1)
                return (int)(minInt + increment); //minimun movement speed + 1/5th of the range

            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                return (int)(minInt + increment * 2); //minimun movement speed + 2/5th of the range

            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                return (int)(minInt + increment * 3); //minimun movement speed + 3/5th of the range

            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                return (int)(minInt + increment * 4); //minimun movement speed + 4/5th of the range

            else if (highFrBandAverage > max)
                return maxInt; ////minimun movement speed + 5/5th of the range = Equal to maximun movement speed :)

            return minInt; //minimun movement speed
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 4: 2 Step Returns Float----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Return the maxFloat value if the min value is exceeded, otherwise return the minFloat value. 
        //Using the low frequency band average.
        public float getFloat_2StepLowFrq(float minFloat, float maxFloat, float min) //Gets movement speed according to the beat of an audio clip, e.g. the low frequencies.
        {
            if (lowFrBandAverage > min)
                return maxFloat;

            return minFloat;
        }

        //Using the mid frequency band average.
        public float getFloat_2StepMidFrq(float minFloat, float maxFloat, float min) //Gets movement speed according to the beat of an audio clip, e.g. the low frequencies.
        {
            if (midFrBandAverage > min)
                return maxFloat;

            return minFloat;
        }

        //Using the high frequency band average.
        public float getFloat_2StepHighFrq(float minFloat, float maxFloat, float min) //Gets movement speed according to the beat of an audio clip, e.g. the low frequencies.
        {
            if (highFrBandAverage > min)
                return maxFloat;

            return minFloat;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 5: 6 Step Return Float----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Return the minFloat value if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call).
        //Return minFloat + increment if min < low/mid/high FrBandAverag <= x1. 
        //Return minFloat + increment * 2 if x1 < low/mid/high FrBandAverag <= x2. 
        //Return minFloat + increment * 3 if x2 < low/mid/high FrBandAverag <= x3. 
        //Return minFloat + increment * 4 if x3 < low/mid/high FrBandAverag >= max. 
        //Return minFloat + increment * 5 if low/mid/high FrBandAverag > max.
        //Depending on the current low / mid / high FrBandAverage each function will return one of 6 different outputs equal to or between the min and max floats. 
        //Using the low frequency band average.
        public float getFloat_6StepLowFrq(float minFloat, float maxFloat, float min, float x1, float x2, float x3, float max) 
        {
            float range = maxFloat - minFloat; //Find the range
            float increment = range / 5; //Divide the range by 5, to create an incremental step value to be used below:

            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                return minFloat + increment; //minimun movement speed + 1/5th of the range
            
            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                return minFloat + increment * 2; //minimun movement speed + 2/5th of the range
            
            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                return minFloat + increment * 3; //minimun movement speed + 3/5th of the range
            
            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                return minFloat + increment * 4; //minimun movement speed + 4/5th of the range
            
            else if (lowFrBandAverage > max)
                return maxFloat; ////minimun movement speed + 5/5th of the range = Equal to maximun movement speed :)

            return minFloat; //minimun movement speed
        }

        //Using the mid frequency band average.
        public float getFloat_6StepMidFrq(float minFloat, float maxFloat, float min, float x1, float x2, float x3, float max) //Gets movement speed still using the beat of an audio clip, but returning a smoothed speed increase / decrease.
        {
            float range = maxFloat - minFloat; //Find the range
            float increment = range / 5; //Divide the range by 5, to create an incremental step value to be used below:

            if (midFrBandAverage > min && midFrBandAverage <= x1)
                return minFloat + increment; //minimun movement speed + 1/5th of the range
            
            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                return minFloat + increment * 2; //minimun movement speed + 2/5th of the range
            
            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                return minFloat + increment * 3; //minimun movement speed + 3/5th of the range
            
            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                return minFloat + increment * 4; //minimun movement speed + 4/5th of the range
            
            else if (midFrBandAverage > max)
                return maxFloat; ////minimun movement speed + 5/5th of the range = Equal to maximun movement speed :)

            return minFloat; //minimun movement speed
        }

        //Using the high frequency band average.
        public float getFloat_6StepHighFrq(float minFloat, float maxFloat, float min, float x1, float x2, float x3, float max) //Gets movement speed still using the beat of an audio clip, but returning a smoothed speed increase / decrease.
        {
            float range = maxFloat - minFloat; //Find the range
            float increment = range / 5; //Divide the range by 5, to create an incremental step value to be used below:

            if (highFrBandAverage > min && highFrBandAverage <= x1)
                return minFloat + increment; //minimun movement speed + 1/5th of the range
            
            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                return minFloat + increment * 2; //minimun movement speed + 2/5th of the range
            
            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                return minFloat + increment * 3; //minimun movement speed + 3/5th of the range
            
            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                return minFloat + increment * 4; //minimun movement speed + 4/5th of the range
            
            else if (highFrBandAverage > max)
                return maxFloat; ////minimun movement speed + 5/5th of the range = Equal to maximun movement speed :)

            return minFloat; //minimun movement speed
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 6: 2 Step Return String----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Using the low frequency band average.
        public String getString_2StepLowFrq(String string1, String string2, float min) //if the minimun value is exceeded return string2, otherwise return string1
        {
            if (lowFrBandAverage > min)
                return string2;

            return string1;
        }

        //Using the mid frequency band average.
        public String getString_2StepMidFrq(String string1, String string2, float min) //if the minimun value is exceeded return string2, otherwise return string1
        {
            if (midFrBandAverage > min)
                return string2;

            return string1;
        }

        //Using the high frequency band average.
        public String getString_2StepHighFrq(String string1, String string2, float min) //if the minimun value is exceeded return string2, otherwise return string1
        {
            if (highFrBandAverage > min)
                return string2;

            return string1;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 7: 6 Step Return String----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns a string depending on the float values:
        //Return string_MinResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call).
        //Return string_LowMediumResponce if min < low/mid/high FrBandAverag <= x1. 
        //Return string_MediumResponce if x1 < low/mid/high FrBandAverag <= x2. 
        //Return string_HighMediumResponce if x2 < low/mid/high FrBandAverag <= x3. 
        //Return string_HighResponce if x3 < low/mid/high FrBandAverag >= max. 
        //Return string_MaxResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public String getString_6StepLowFrq(String string_MinResponce, String string_LowMediumResponce, String string_MediumResponce,
            String string_HighMediumResponce, String string_HighResponce, String string_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                return string_LowMediumResponce;

            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                return string_MediumResponce;

            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                return string_HighMediumResponce;

            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                return string_HighResponce;

            else if (lowFrBandAverage > max)
                return string_MaxResponce;

            return string_MinResponce;
        }

        //Using the mid frequency band average.
        public String getString_6StepMidFrq(String string_MinResponce, String string_LowMediumResponce, String string_MediumResponce,
            String string_HighMediumResponce, String string_HighResponce, String string_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (midFrBandAverage > min && midFrBandAverage <= x1)
                return string_LowMediumResponce;

            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                return string_MediumResponce;

            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                return string_HighMediumResponce;

            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                return string_HighResponce;

            else if (midFrBandAverage > max)
                return string_MaxResponce;

            return string_MinResponce;
        }

        //Using the high frequency band average.
        public String getString_6StepHighFrq(String string_MinResponce, String string_LowMediumResponce, String string_MediumResponce,
            String string_HighMediumResponce, String string_HighResponce, String string_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (highFrBandAverage > min && highFrBandAverage <= x1)
                return string_LowMediumResponce;

            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                return string_MediumResponce;

            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                return string_HighMediumResponce;

            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                return string_HighResponce;

            else if (highFrBandAverage > max)
                return string_MaxResponce;

            return string_MinResponce;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 8: 3 Step Return Vector2------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA Vector2 depending on the float values:
        //Return minVector2 if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call). 
        //Return midVector2 if min < low/mid/high FrBandAverag >= max. 
        //Return maxVector2 if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public Vector2 getVector2_3StepLowFrq(Vector2 minVector2, Vector2 midVector2, Vector2 maxVector2,
            float min, float max)
        {
            if (lowFrBandAverage > min && lowFrBandAverage <= max)
                return midVector2;
            
            else if (lowFrBandAverage > max)
                return maxVector2;

            return minVector2;
        }

        //Using the mid frequency band average.
        public Vector2 getVector2_3StepMidFrq(Vector2 minVector2, Vector2 midVector2, Vector2 maxVector2,
            float min, float max)
        {
            if (midFrBandAverage > min && midFrBandAverage <= max)
                return midVector2;

            else if (midFrBandAverage > max)
                return maxVector2;

            return minVector2;
        }

        //Using the high frequency band average.
        public Vector2 getVector2_3StepHighFrq(Vector2 minVector2, Vector2 midVector2, Vector2 maxVector2,
            float min, float max)
        {
            if (highFrBandAverage > min && highFrBandAverage <= max)
                return midVector2;

            else if (highFrBandAverage > max)
                return maxVector2;

            return minVector2;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 9: 6 Step Return Vector2------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA Vector2 depending on the float values: 
        //Return vector2_MinResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call).
        //Return vector2_LowMediumResponce if min < low/mid/high FrBandAverag <= x1. 
        //Return vector2_MediumResponce if x1 < low/mid/high FrBandAverag <= x2. 
        //Return vector2_HighMediumResponce if x2 < low/mid/high FrBandAverag <= x3. 
        //Return vector2_HighResponce if x3 < low/mid/high FrBandAverag >= max. 
        //Return vector2_MaxResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public Vector2 getVector2_6StepLowFrq(Vector2 vector2_MinResponce, Vector2 vector2_LowMediumResponce, Vector2 vector2_MediumResponce,
            Vector2 vector2_HighMediumResponce, Vector2 vector2_HighResponce, Vector2 vector2_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                return vector2_LowMediumResponce;

            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                return vector2_MediumResponce;

            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                return vector2_HighMediumResponce;

            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                return vector2_HighResponce;

            else if (lowFrBandAverage > max)
                return vector2_MaxResponce;

            //Low responce
            return vector2_MinResponce;
        }

        //Using the mid frequency band average.
        public Vector2 getVector2_6StepMidFrq(Vector2 vector2_MinResponce, Vector2 vector2_LowMediumResponce, Vector2 vector2_MediumResponce,
            Vector2 vector2_HighMediumResponce, Vector2 vector2_HighResponce, Vector2 vector2_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (midFrBandAverage > min && midFrBandAverage <= x1)
                return vector2_LowMediumResponce;

            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                return vector2_MediumResponce;

            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                return vector2_HighMediumResponce;

            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                return vector2_HighResponce;

            else if (midFrBandAverage > max)
                return vector2_MaxResponce;

            //Low responce
            return vector2_MinResponce;
        }

        //Using the high frequency band average.
        public Vector2 getVector2_6StepHighFrq(Vector2 vector2_MinResponce, Vector2 vector2_LowMediumResponce, Vector2 vector2_MediumResponce,
            Vector2 vector2_HighMediumResponce, Vector2 vector2_HighResponce, Vector2 vector2_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (highFrBandAverage > min && highFrBandAverage <= x1)
                return vector2_LowMediumResponce;

            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                return vector2_MediumResponce;

            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                return vector2_HighMediumResponce;

            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                return vector2_HighResponce;

            else if (highFrBandAverage > max)
                return vector2_MaxResponce;

            //Low responce
            return vector2_MinResponce;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 10: 3 Step Return TimeSpan------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA TimeSpan depending on the float values:
        //Return minimunInterval if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call). 
        //Return midInterval if min < low/mid/high FrBandAverag >= max. 
        //Return maximunInterval if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public TimeSpan getTimeSpan_3StepLowFrq(float minimunInterval, float midInterval, float maximunInterval,
            float min, float max) //Need to normalize this......
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(maximunInterval);

            if (lowFrBandAverage > min && lowFrBandAverage <= max)
                timeSpan = TimeSpan.FromSeconds(midInterval);

            else if (lowFrBandAverage > max)
                timeSpan = TimeSpan.FromSeconds(minimunInterval);

            return timeSpan;
        }

        //Using the mid frequency band average.
        public TimeSpan getTimeSpan_3StepMidFrq(float minimunInterval, float midInterval, float maximunInterval,
            float min, float max) //Need to normalize this......
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(maximunInterval);

            if (midFrBandAverage > min && midFrBandAverage <= max)
                timeSpan = TimeSpan.FromSeconds(midInterval);

            else if (midFrBandAverage > max)
                timeSpan = TimeSpan.FromSeconds(minimunInterval);

            return timeSpan;
        }

        //Using the high frequency band average.
        public TimeSpan getTimeSpan_3StepHighFrq(float minimunInterval, float midInterval, float maximunInterval,
            float min, float max) //Need to normalize this......
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(maximunInterval);

            if (highFrBandAverage > min && highFrBandAverage <= max)
                timeSpan = TimeSpan.FromSeconds(midInterval);

            else if (highFrBandAverage > max)
                timeSpan = TimeSpan.FromSeconds(minimunInterval);

            return timeSpan;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 11: 6 Step Return TimeSpan------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA TimeSpan depending on the float values: 
        //Return TimeSpan_MinResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call).
        //Return TimeSpan_LowMediumResponce if min < low/mid/high FrBandAverag <= x1. 
        //Return TimeSpan_MediumResponce if x1 < low/mid/high FrBandAverag <= x2. 
        //Return TimeSpan_HighMediumResponce if x2 < low/mid/high FrBandAverag <= x3. 
        //Return TimeSpan_HighResponce if x3 < low/mid/high FrBandAverag >= max. 
        //Return TimeSpan_MaxResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public TimeSpan getTimeSpan_6StepLowFrq(float minInterval, float lowMediumInterval, float mediumInterval,
            float highMediumInterval, float highInterval, float maxInterval,
            float min, float x1, float x2, float x3, float max)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(maxInterval);

            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                timeSpan = TimeSpan.FromSeconds(highInterval);    

            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                timeSpan = TimeSpan.FromSeconds(highMediumInterval);  

            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                timeSpan = TimeSpan.FromSeconds(mediumInterval); 

            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                timeSpan = TimeSpan.FromSeconds(lowMediumInterval);  

            else if (lowFrBandAverage > max)
                timeSpan = TimeSpan.FromSeconds(minInterval);       

            //Low responce
            return timeSpan;
        }

        //Using the mid frequency band average.
        public TimeSpan getTimeSpan_6StepMidFrq(float minInterval, float lowMediumInterval, float mediumInterval,
            float highMediumInterval, float highInterval, float maxInterval,
            float min, float x1, float x2, float x3, float max)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(maxInterval);

            if (midFrBandAverage > min && midFrBandAverage <= x1)
                timeSpan = TimeSpan.FromSeconds(highInterval);

            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                timeSpan = TimeSpan.FromSeconds(highMediumInterval);

            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                timeSpan = TimeSpan.FromSeconds(mediumInterval);

            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                timeSpan = TimeSpan.FromSeconds(lowMediumInterval);

            else if (midFrBandAverage > max)
                timeSpan = TimeSpan.FromSeconds(minInterval);

            //Low responce
            return timeSpan;
        }

        //Using the high frequency band average.
        public TimeSpan getTimeSpan_6StepHighFrq(float minInterval, float lowMediumInterval, float mediumInterval,
            float highMediumInterval, float highInterval, float maxInterval,
            float min, float x1, float x2, float x3, float max)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(maxInterval);

            if (highFrBandAverage > min && highFrBandAverage <= x1)
                timeSpan = TimeSpan.FromSeconds(highInterval);

            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                timeSpan = TimeSpan.FromSeconds(highMediumInterval);

            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                timeSpan = TimeSpan.FromSeconds(mediumInterval);

            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                timeSpan = TimeSpan.FromSeconds(lowMediumInterval);

            else if (highFrBandAverage > max)
                timeSpan = TimeSpan.FromSeconds(minInterval);

            //Low responce
            return timeSpan;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 12: 3 Step Return Color-----------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA Color depending on the float values:
        //Return colour_LowResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call). 
        //Return colour_MediumResponce if min < low/mid/high FrBandAverag >= max. 
        //Return colour_HighResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public Color getColour_3StepLowFrq(Color colour_LowResponce, Color colour_MediumResponce, Color colour_HighResponce,
            float min, float max) 
        {
            //Medium responce:
            if (lowFrBandAverage > min && lowFrBandAverage <= max)
                return colour_MediumResponce;

            //High responce dependent on the average amplitude of the low frq band: <------------------
            else if (lowFrBandAverage > max)
                return colour_HighResponce;

            //Low responce
            return colour_LowResponce;
        }

        //Using the mid frequency band average.
        public Color getColour_3StepMidFrq(Color colour_LowResponce, Color colour_MediumResponce, Color colour_HighResponce,
            float min, float max)
        {
            //Medium responce:
            if (midFrBandAverage > min && midFrBandAverage <= max)
                return colour_MediumResponce;

            //High responce dependent on the average amplitude of the low frq band: <------------------
            else if (midFrBandAverage > max)
                return colour_HighResponce;

            //Low responce
            return colour_LowResponce;
        }

        //Using the high frequency band average.
        public Color getColour_3StepHighFrq(Color colour_LowResponce, Color colour_MediumResponce, Color colour_HighResponce,
            float min, float max)
        {
            //Medium responce:
            if (highFrBandAverage > min && highFrBandAverage <= max)
                return colour_MediumResponce;

            //High responce dependent on the average amplitude of the low frq band: <------------------
            else if (highFrBandAverage > max)
                return colour_HighResponce;

            //Low responce
            return colour_LowResponce;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 13: 6 Step Return Color-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA Color depending on the float values: 
        //Return colour_MinResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call).
        //Return colour_LowMediumResponce if min < low/mid/high FrBandAverag <= x1. 
        //Return colour_MediumResponce if x1 < low/mid/high FrBandAverag <= x2. 
        //Return colour_HighMediumResponce if x2 < low/mid/high FrBandAverag <= x3. 
        //Return colour_HighResponce if x3 < low/mid/high FrBandAverag >= max. 
        //Return colour_MaxResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public Color getColour_6StepLowFrq(Color colour_MinResponce, Color colour_LowMediumResponce, Color colour_MediumResponce,
            Color colour_HighMediumResponce, Color colour_HighResponce, Color colour_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                return colour_LowMediumResponce;

            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                return colour_MediumResponce;

            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                return colour_HighMediumResponce;

            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                return colour_HighResponce;

            else if (lowFrBandAverage > max)
                return colour_MaxResponce;

            //Low responce
            return colour_MinResponce;
        }

        //Using the mid frequency band average.
        public Color getColour_6StepMidFrq(Color colour_MinResponce, Color colour_LowMediumResponce, Color colour_MediumResponce,
            Color colour_HighMediumResponce, Color colour_HighResponce, Color colour_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (midFrBandAverage > min && midFrBandAverage <= x1)
                return colour_LowMediumResponce;

            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                return colour_MediumResponce;

            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                return colour_HighMediumResponce;

            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                return colour_HighResponce;

            else if (midFrBandAverage > max)
                return colour_MaxResponce;

            //Low responce
            return colour_MinResponce;
        }

        //Using the high frequency band average.
        public Color getColour_6StepHighFrq(Color colour_MinResponce, Color colour_LowMediumResponce, Color colour_MediumResponce,
            Color colour_HighMediumResponce, Color colour_HighResponce, Color colour_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (highFrBandAverage > min && highFrBandAverage <= x1)
                return colour_LowMediumResponce;

            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                return colour_MediumResponce;

            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                return colour_HighMediumResponce;

            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                return colour_HighResponce;

            else if (highFrBandAverage > max)
                return colour_MaxResponce;

            //Low responce
            return colour_MinResponce;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 14: 3 Step Return Color[]----------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA Color[] depending on the float values:
        //Return colourArray_LowResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call). 
        //Return colourArray_MediumResponce if min < low/mid/high FrBandAverag >= max. 
        //Return colourArray_HighResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public Color[] getColourArray_3StepLowFrq(Color[] colourArray_LowResponce, Color[] colourArray_MediumResponce, Color[] colourArray_HighResponce,
            float min, float max) 
        {          
            //Medium responce:
            if (lowFrBandAverage > min && lowFrBandAverage <= max)
                return colourArray_MediumResponce;  

            //High responce:
            else if (lowFrBandAverage > max)
                return colourArray_HighResponce;

            //Low responce
            return colourArray_LowResponce;
        }

        //Using the mid frequency band average.
        public Color[] getColourArray_3StepMidFrq(Color[] colourArray_LowResponce, Color[] colourArray_MediumResponce, Color[] colourArray_HighResponce,
            float min, float max)
        {
            //Medium responce:
            if (midFrBandAverage > min && midFrBandAverage <= max)
                return colourArray_MediumResponce;

            //High responce:
            else if (midFrBandAverage > max)
                return colourArray_HighResponce;

            //Low responce
            return colourArray_LowResponce;
        }

        //Using the high frequency band average.
        public Color[] getColourArray_3StepHighFrq(Color[] colourArray_LowResponce, Color[] colourArray_MediumResponce, Color[] colourArray_HighResponce,
            float min, float max)
        {
            //Medium responce:
            if (highFrBandAverage > min && highFrBandAverage <= max)
                return colourArray_MediumResponce;

            //High responce:
            else if (highFrBandAverage > max)
                return colourArray_HighResponce;

            //Low responce
            return colourArray_LowResponce;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------FUNCTION SET 15: 6 Step Return Color[]-----------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //returns an XNA Color[] depending on the float values: 
        //Return colourArray_MinResponce if the min value is not exceeded by the low/mid/high FrBandAverage (depending on the function call).
        //Return colourArray_LowMediumResponce if min < low/mid/high FrBandAverag <= x1. 
        //Return colourArray_MediumResponce if x1 < low/mid/high FrBandAverag <= x2. 
        //Return colourArray_HighMediumResponce if x2 < low/mid/high FrBandAverag <= x3. 
        //Return colourArray_HighResponce if x3 < low/mid/high FrBandAverag >= max. 
        //Return colourArray_MaxResponce if low/mid/high FrBandAverag > max.
        //Using the low frequency band average.
        public Color[] getColourArray_6StepLowFrq(Color[] colourArray_MinResponce, Color[] colourArray_LowMediumResponce, Color[] colourArray_MediumResponce,
            Color[] colourArray_HighMediumResponce, Color[] colourArray_HighResponce, Color[] colourArray_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (lowFrBandAverage > min && lowFrBandAverage <= x1)
                return colourArray_LowMediumResponce;

            else if (lowFrBandAverage > x1 && lowFrBandAverage <= x2)
                return colourArray_MediumResponce;

            else if (lowFrBandAverage > x2 && lowFrBandAverage <= x3)
                return colourArray_HighMediumResponce;

            else if (lowFrBandAverage > x3 && lowFrBandAverage <= max)
                return colourArray_HighResponce;

            else if (lowFrBandAverage > max)
                return colourArray_MaxResponce;

            //Low responce
            return colourArray_MinResponce;
        }

        //Using the mid frequency band average.
        public Color[] getColourArray_6StepMidFrq(Color[] colourArray_MinResponce, Color[] colourArray_LowMediumResponce, Color[] colourArray_MediumResponce,
            Color[] colourArray_HighMediumResponce, Color[] colourArray_HighResponce, Color[] colourArray_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (midFrBandAverage > min && midFrBandAverage <= x1)
                return colourArray_LowMediumResponce;

            else if (midFrBandAverage > x1 && midFrBandAverage <= x2)
                return colourArray_MediumResponce;

            else if (midFrBandAverage > x2 && midFrBandAverage <= x3)
                return colourArray_HighMediumResponce;

            else if (midFrBandAverage > x3 && midFrBandAverage <= max)
                return colourArray_HighResponce;

            else if (midFrBandAverage > max)
                return colourArray_MaxResponce;

            //Low responce
            return colourArray_MinResponce;
        }

        //Using the high frequency band average.
        public Color[] getColourArray_6StepHighFrq(Color[] colourArray_MinResponce, Color[] colourArray_LowMediumResponce, Color[] colourArray_MediumResponce,
            Color[] colourArray_HighMediumResponce, Color[] colourArray_HighResponce, Color[] colourArray_MaxResponce,
            float min, float x1, float x2, float x3, float max)
        {
            if (highFrBandAverage > min && highFrBandAverage <= x1)
                return colourArray_LowMediumResponce;

            else if (highFrBandAverage > x1 && highFrBandAverage <= x2)
                return colourArray_MediumResponce;

            else if (highFrBandAverage > x2 && highFrBandAverage <= x3)
                return colourArray_HighMediumResponce;

            else if (highFrBandAverage > x3 && highFrBandAverage <= max)
                return colourArray_HighResponce;

            else if (highFrBandAverage > max)
                return colourArray_MaxResponce;

            //Low responce
            return colourArray_MinResponce;
        }

    }
}
