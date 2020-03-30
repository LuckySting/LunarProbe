using System;

namespace LunarProbe
{
    /// <summary>
    /// Интерфейс данных
    /// </summary>
    interface Data
    {
        /// <summary>
        /// Получить данные из объекта
        /// </summary>
        /// <param name="lastValue">Последнее сохраненное значение</param>
        /// <param name="currentNumber">Текущий номер измерения</param>
        /// <param name="currentMean">Текущее среднее по всем измерениям</param>
        /// <returns>Новые данные</returns>
        public int getData(int lastValue, ref UInt64 currentNumber, ref Double currentMean);
    }

    /// <summary>
    /// Реализация интерфеса данных, представляющий нормальные данные
    /// </summary>
    class NormalData: Data
    {
        private int value;
        public NormalData(int value)
        {
            this.value = value;
        }
        public int getData(int lastValue, ref UInt64 currentNumber, ref Double currentMean)
        {
            Double coefficient = currentNumber / (currentNumber + 1);

            Double additional = this.value / (Convert.ToInt64(currentNumber) + 1);

            currentMean = Math.Round(currentMean * coefficient + additional, 3);

            currentNumber += 1;

            return this.value;
        }
    }

    /// <summary>
    /// Реализация интерфеса данных, представляющий пустые данные (шаблон пустой объект)
    /// </summary>
    class NullData : Data
    {
        /// <summary>
        /// Получение данных из пустого объекта
        /// </summary>
        /// <param name="lastValue">Последнее значение будет вернуто назад</param>
        /// <param name="currentNumber">Текущий номер измерения не изменится</param>
        /// <param name="currentMean">Текущее среднее не изменится</param>
        /// <returns>Последнее значение, т.к нового нет</returns>
        public int getData(int lastValue, ref UInt64 currentNumber, ref Double currentMean)
        {
            return lastValue;
        }
    }

    /// <summary>
    /// Интерфейс датчика
    /// </summary>
    interface Sensor
    {
        public Data getData(); 
    }

    /// <summary>
    /// Реализация интерфейса датчика, представляющая заглушку для тестов
    /// </summary>
    class StubSensor : Sensor
    {
        private int minValue;
        private int maxValue;
        private Byte errorPercent;
        private Random random;

        public StubSensor(int minValue, int maxValue, Byte errorPercent = 30)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.errorPercent = errorPercent;
            this.random = new Random();
        }

        public Data getData()
        {
            
            if (random.Next(0, 100) > errorPercent)
            {
                return new NormalData(random.Next(minValue, maxValue));
            } else
            {
                return new NullData();
            }
        }
    }
    class Program
    {
        static Sensor temperature;
        static Sensor pressure;
        static Sensor seismography;
        static Sensor groundPh;
        static Sensor humidity;
        static void Main(string[] args)
        {
            UInt64 numberTemp = 0;
            Double meanTemp = 0;
            int temp = 0;
            temperature = new StubSensor(-200, 200);

            UInt64 numberPres = 0;
            Double meanPres = 500;
            int pres = 500;
            pressure = new StubSensor(0, 1000);

            UInt64 numberPh = 0;
            Double meanPh = 5;
            int Ph = 5;
            groundPh = new StubSensor(0, 10);

            UInt64 numberSeis = 0;
            Double meanSeis = 5;
            int seis = 5;
            seismography = new StubSensor(0, 10);

            UInt64 numberHum = 0;
            Double meanHum = 5;
            int hum = 5;
            humidity = new StubSensor(0, 100);

            while (true) {
                Console.Clear();
                temp = temperature.getData().getData(temp, ref numberTemp, ref meanTemp);
                Console.WriteLine("Current temp: " + temp + ", Mean temp = " + meanTemp);
                pres = temperature.getData().getData(pres, ref numberPres, ref meanPres);
                Console.WriteLine("Current pres: " + pres + ", Mean pres = " + meanPres);
                Ph = temperature.getData().getData(Ph, ref numberPh, ref meanPh);
                Console.WriteLine("Current Ph: " + Ph + ", Mean Ph = " + meanPh);
                seis = temperature.getData().getData(seis, ref numberSeis, ref meanSeis);
                Console.WriteLine("Current seis: " + seis + ", Mean seis = " + meanSeis);
                hum = temperature.getData().getData(hum, ref numberHum, ref meanHum);
                Console.WriteLine("Current hum: " + hum + ", Mean hum = " + meanHum);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
