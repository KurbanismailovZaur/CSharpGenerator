# EditorScriptsTemplates
This project will help you quickly create the code files of the template you want.

There are quick menus for the following types:
- `classes`.
- `structures`
- `interfaces`
- `enumerations`
- `scripts`.

To use the desired template, use the `Assets` > `Create` menu, or right-click in the project tab and then hover over the `Create` menu.

![image](https://user-images.githubusercontent.com/5365111/157711668-419d0d02-df98-4a4f-b4bc-cdabff2523a9.png)

This asset automatically generates a namespace inside the created templates, taking into account the name of the project + path where the template is created. If the path to the template being created contains the folder `Scripts` it will be considered as the starting point for generation of the namespace, while the folder `Scripts` itself will not exist in the namespace path.

The screenshot below shows the generation of the `SomeClass` template called with the menu `Create` > `C# Class`. Notice (look at the orange arrows) that the generated namespace (3) includes the project name (EditorScriptsTemplates), but does not include the folder name `Scripts` (1).

![image](https://user-images.githubusercontent.com/5365111/157711239-94f4d04a-ed3b-4da8-aa21-5e0aa3699d99.png)

However, if you create a template in a subfolder (1), it will be included in the generated namespace path (2). 

![image](https://user-images.githubusercontent.com/5365111/157768183-5cdad1c8-3339-4412-be1a-5c559bc295da.png)

This approach allows you to avoid wasting time changing the generated code templates; instead, you get what you want to create right away.

Generating namespace by folder allows you to keep your code architecture clean; you don't have to make up namespace names, but make up proper folder names instead. In the future, when your project grows to gigantic proportions, it will be easier for you to navigate because the folder and namespace names in your code files will match and have meaningful names.

Good luck!
