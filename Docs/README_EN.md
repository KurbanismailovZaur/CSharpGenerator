# Create Menu Context

This project will help you quickly create files with the code of the template you need. You can configure which default namespaces to include in code files and also whether you need to generate a namespace depending on the folder hierarchy.

There are quick menus for the following types:
- `classes`
- `structures`
- `interfaces`
- `enumerations`
- `scripts`
- `ScriptableObject`

To use the desired template use the `Assets` > `Create` menu, or right-click in the project tab and then hover over the `Create` menu.

![image](https://user-images.githubusercontent.com/5365111/200543731-31f671be-95ce-4440-97a4-0cf27a0a20f5.png)

You can set up pluggable namespaces and their generation in code files by calling `Windows` menu > `C# Generator` > `Settings`.

![image](https://user-images.githubusercontent.com/5365111/200572355-a7a55c1a-013a-42c1-818f-7ae653af6709.png)

In the window that appears, you will see the namespace generation settings.

![image](https://user-images.githubusercontent.com/5365111/200572458-f158daec-6c00-4de2-a9e4-26e8711a38f7.png)

- `Generate Namespaces` - responsible for generating the namespace when creating the code file. The generated namespace depends on the location of the file in the hierarchy.
  - If the `Scripts` folder is present in the path of the code file, then it will be considered the starting point for generating the namespace, but instead of the `Scripts` folder, the name of the project will be inserted into the namespace.
  - Otherwise, the `Assets` folder will be considered as the starting point of the path, but instead of the `Assets` folder, the name of the project will be inserted into the namespace.
- `Default Usings` - namespaces included by default in any code file.

> The project name is taken from `Project Settings` > `Player`> `Product Name`.

### Namespace generation example
Create a folder `Scripts` > `Vehicles`, and in it using the menu `Create` > `C# MonoBehaviour` code files `Car1` and `Car2`. However, when you create `Car1`, then let the option `Generate Namespaces` in the settings `Windows` > `Create Menu Context` > `Settings` be on, and for `Car2` - off.

![image](https://user-images.githubusercontent.com/5365111/200547315-8bf04464-09ea-45be-b0c3-32845fa846da.png)

As a result, the following code will be generated in `Car1`:
```C#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace MyGame.Vehicles
{
    public class Car1 : MonoBehavior
    {
    }
}
```

And in `Car2` this:
```C#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class Car2 : MonoBehavior
{
}
```

Namespace generation across folders keeps the code architecture clean, you don't have to come up with names for the namespaces, instead come up with proper folder names. In the future, when your project grows to a gigantic size, it will be easier for you to navigate it, since the names of folders and namespaces in the code files will match and have meaningful names.

Good luck!
