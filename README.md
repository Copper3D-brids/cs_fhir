# cs_fhir

## Discussion

[Zulip](https://chat.fhir.org/)

## Packages

- `Hl7.FHIR.R4`: 3.4.0
- `Hl7.fhir.specification.r4`: 3.4.0
- [Fluent validation](https://fluentvalidation.net): 10.3.0

```sh
Install-Package Hl7.FHIR.R4 -version 3.4.0
NuGet\Install-Package Hl7.Fhir.Specification.R4 -Version 3.4.0
NuGet\Install-Package System.CommandLine.DragonFruit -Version 0.4.0-alpha.22272.1
```

## Validator

- Way 1: validate via online tools
    - [online validator](https://inferno.healthit.gov/validator/)

- Way 2: validate via jave jar
    - download a validator from [HL7 validator](https://hl7.org/fhir/validator/)
    - check the [validation code](https://hl7.org/fhir/R4/validation.html)
    - [us.core](https://www.hl7.org/fhir/us/core/StructureDefinition-us-core-patient.html)
```sh
    java -jar .\validator_cli.jar .\patient.json -version 4.0.1
```

- Way 3: validate via [fhirly](https://simplifier.net/downloads/firely-terminal)

```sh
    dotnet tool install -g firely.terminal
```

```sh
    fhir push .\patient.json
```