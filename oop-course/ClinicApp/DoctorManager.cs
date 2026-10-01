namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count => _count;

    public Doctor? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _doctors[index];
        }
    }

    public void Add(Doctor doctor)
    {
        if (_count >= MaxDoctors)
        {
            Console.WriteLine("Error: Limit of doctors reached.");
            return;
        }

        _doctors[_count] = doctor;
        _count++;
        Console.WriteLine($"Doctor [{doctor.Id}] {doctor.FullName} added.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }

        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        string query = speciality.ToLower();
        int matchesCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToString().ToLower().Contains(query))
            {
                matchesCount++;
            }
        }

        Doctor[] result = new Doctor[matchesCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality.ToString().ToLower().Contains(query))
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }

    public Doctor[] FindBySpeciality(Speciality speciality)
    {
        int matchesCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                matchesCount++;
            }
        }

        Doctor[] result = new Doctor[matchesCount];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Speciality == speciality)
            {
                result[index++] = _doctors[i];
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] copy = new Doctor[_count];
        Array.Copy(_doctors, copy, _count);
        return copy;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
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
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;
        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Doctor list is empty.");
            return;
        }

        Console.WriteLine($"=== Doctors ({_count} / {MaxDoctors}) ===");

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i]);
        }

        Console.WriteLine(new string('-', 60));
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("No data for statistics.");
            return;
        }

        int availableCount = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableCount++;
            }
        }

        Console.WriteLine("=== Doctor Statistics ===");
        Console.WriteLine($"Total:           {_count}");
        Console.WriteLine($"Available now:   {availableCount}");
        Console.WriteLine("By speciality:");

        for (int i = 0; i < _count; i++)
        {
            bool isUnique = true;

            for (int j = 0; j < i; j++)
            {
                if (_doctors[i].Speciality == _doctors[j].Speciality)
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                int countForSpeciality = 0;

                for (int k = 0; k < _count; k++)
                {
                    if (_doctors[i].Speciality == _doctors[k].Speciality)
                    {
                        countForSpeciality++;
                    }
                }

                Console.WriteLine($"  {_doctors[i].Speciality}: {countForSpeciality}");
            }
        }

        Console.WriteLine("==========================");
    }

    public bool TryFindById(int id, out Doctor doctor)
    {
        Doctor? foundDoctor = FindById(id);

        if (foundDoctor != null)
        {
            doctor = foundDoctor;
            return true;
        }

        doctor = null!;
        return false;
    }
}