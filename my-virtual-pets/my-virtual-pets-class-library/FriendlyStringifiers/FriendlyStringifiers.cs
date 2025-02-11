using my_virtual_pets_class_library.Enums;

namespace my_virtual_pets_class_library.FriendlyStringifiers;

public static class FriendlyStringifiers
{
    public static string FriendlyStringify(this PetType petType)
    {
        return petType.ToString().ToLower().Replace('_', ' ');
    }
}