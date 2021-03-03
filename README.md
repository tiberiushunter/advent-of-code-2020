# :christmas_tree: Advent of Code

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
![Project Maintenance][maintenance-shield]
[![MIT License][license-shield]][license-url]

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]

<!-- ABOUT -->
## About

A repository for my solutions to [Advent of Code][aoc]. This project is a C# console application with [.NET 5][.net] to solve all the solutions from 2015 to 2020.

Feel free to run through the solutions (*Note: Potential Spoilers!*)


<!-- GETTING STARTED -->
## Getting Started

To run the code ensure you have the [.NET 5 SDK][.net-sdk]

Then from the root directory, run the command `dotnet restore` to ensure you have the required NuGet packages installed.

Next, grab your Advent of Code session key. In Chrome this can be found under the Application tab in DevTools as seen below: 

![AoCSession](https://user-images.githubusercontent.com/10655940/109741093-4c5e7f80-7bc4-11eb-9e58-e463c64258a5.png)

Once you've got this session token, you can add it to the UserSecrets with the name "AdventOfCode:Session" with the below command:

`dotnet user-secrets set "AdventOfCode:Session" "y0ur_s3ss10n_k3y*"`

This is used to store your Advent of Code session key to fetch the input directly from the site rather than adding it in yourself.

Finally, run `dotnet run` to launch the application.

You'll then be presented with a Welcome Screen where you will be prompted to input a year and a day (or press Enter at both prompts to run all of the Advent of Code solutions) to see the results.

<!-- CONTRIBUTING -->
## Contributing

As this repository is my own solutions I might be *slightly* hesitant for adding contributions by others but feel free to reach out or raise an issue if you find a bug :heart:

<!-- LICENSE -->
## License

Distributed under the MIT License. See [LICENSE][license-url] for more information.

<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements

* [rxaiver's GitHub Emoji Cheat Sheet][1]
* [Img Shields][2]
* [Choose an Open Source License][3]
* [othneildrew's Best README Template][4]

<!-- CONTACT -->
## Contact

Owner: Sam Welek

[![GitHub][github-shield]][github-url]
[![LinkedIn][linkedin-shield]][linkedin-url]
[![Twitter][twitter-shield]][twitter-url]

<a href="https://www.buymeacoffee.com/tiberiushunter" target="_blank"> <img src="https://cdn.buymeacoffee.com/buttons/default-yellow.png" alt="Buy Me A Coffee" height="41" width="174"></a>

Project Link: [GitHub][project-url]

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

<!-- Project Specific -->
[project-url]: https://github.com/tiberiushunter/advent-of-code/

[aoc]: https://adventofcode.com/
[.net]: https://dotnet.microsoft.com/
[.net-sdk]: https://dotnet.microsoft.com/download/

[maintenance-shield]: https://img.shields.io/maintenance/yes/2021.svg?style=for-the-badge

[contributors-shield]: https://img.shields.io/github/contributors/tiberiushunter/advent-of-code.svg?style=for-the-badge
[contributors-url]: https://github.com/tiberiushunter/advent-of-code/graphs/contributors

[forks-shield]: https://img.shields.io/github/forks/tiberiushunter/advent-of-code.svg?style=for-the-badge
[forks-url]: https://github.com/tiberiushunter/advent-of-code/network/members

[stars-shield]: https://img.shields.io/github/stars/tiberiushunter/advent-of-code.svg?style=for-the-badge
[stars-url]: https://github.com/tiberiushunter/advent-of-code/stargazers

[issues-shield]: https://img.shields.io/github/issues/tiberiushunter/advent-of-code.svg?style=for-the-badge
[issues-url]: https://github.com/tiberiushunter/advent-of-code/issues

[license-shield]: https://img.shields.io/github/license/tiberiushunter/advent-of-code.svg?style=for-the-badge
[license-url]: https://github.com/tiberiushunter/advent-of-code/blob/main/LICENSE

<!-- Contact Specific -->
[github-shield]: https://img.shields.io/badge/-GitHub-black.svg?style=for-the-badge&logo=github&colorB=555
[github-url]: https://github.com/tiberiushunter

[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/sam-welek

[twitter-shield]: https://img.shields.io/badge/-Twitter-black.svg?style=for-the-badge&logo=twitter&colorB=555
[twitter-url]: https://twitter.com/samwelek

<!-- Acknowledgement Specific -->
[1]: https://gist.github.com/rxaviers/7360908
[2]: https://shields.io
[3]: https://choosealicense.com
[4]: https://github.com/othneildrew/Best-README-Template
