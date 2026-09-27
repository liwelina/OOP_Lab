namespace Lab03;

public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count => _count;

    public void Add(Patient patient)
    {
        if (_count >= MaxPatients)
        {
            Console.WriteLine("Error: Limit of patients reached.");
            return;
        }

        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} add.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public Patient[] FindByName(string name)
    {
        string query = name.ToLower();
        int matchesCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(query) ||
                _patients[i].LastName.ToLower().Contains(query))
            {
                matchesCount++;
            }
        }

        Patient[] result = new Patient[matchesCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].FirstName.ToLower().Contains(query) ||
                _patients[i].LastName.ToLower().Contains(query))
            {
                result[index++] = _patients[i];
            }
        }

        return result;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1)
        {
            return false;
        }

        for (int i = targetIndex; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        _patients[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Patient list is empty");
            return;
        }

        Console.WriteLine($"=== Patients ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i]);
        }
        Console.WriteLine(new string('─', 60));
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("No data available for statistics");
            return;
        }

        int totalAge = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;
        int adultsCount = 0;

        for (int i = 0; i < _count; i++)
        {
            totalAge += _patients[i].Age;

            if (_patients[i].Age < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (_patients[i].Age > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }

            if (_patients[i].IsAdult)
            {
                adultsCount++;
            }
        }

        double averageAge = (double)totalAge / _count;

        Console.WriteLine("=== Patient statistics ===");
        Console.WriteLine($"Count:       {_count}");
        Console.WriteLine($"Average Age: {averageAge:F1} р.");
        Console.WriteLine($"Youngest:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Oldest:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Adults:     {adultsCount} з {_count}");
        Console.WriteLine("============================");
    }
}