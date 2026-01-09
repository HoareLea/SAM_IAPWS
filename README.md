[![Build (Windows)](https://github.com/SAM-BIM/SAM_IAPWS/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/SAM-BIM/SAM_IAPWS/actions/workflows/build.yml)
[![Installer (latest)](https://img.shields.io/github/v/release/SAM-BIM/SAM_Deploy?label=installer)](https://github.com/SAM-BIM/SAM_Deploy/releases/latest)

# SAM_IAPWS

<a href="https://github.com/SAM-BIM/SAM">
  <img src="https://github.com/SAM-BIM/SAM/blob/master/Grasshopper/SAM.Core.Grasshopper/Resources/SAM_Small.png"
       align="left" hspace="10" vspace="6">
</a>

**SAM_IAPWS** is part of the **SAM (Sustainable Analytical Model) Toolkit** —  
an open-source collection of tools designed to help engineers create, manage,
and process analytical building models for energy and environmental analysis.

This repository provides **thermodynamic property calculations for pure water and steam**
based on the **IAPWS-IF97** industrial formulation published by the  
*International Association for the Properties of Water and Steam (IAPWS)*.

The implementation is intended for applications involving **liquid water, steam,
and supercritical states**, such as plant systems, thermal processes,
and energy simulation workflows within SAM.

---

## Scope

`SAM_IAPWS` implements the **IAPWS-IF97 formulation**, which applies to:

- compressed and subcooled liquid water  
- saturated liquid–vapor phase boundary  
- superheated steam  
- dense and supercritical water states  

It is designed for **pure water and steam only**.

For **humid air and air–water vapor mixtures**, psychrometric models such as  
**SAM_Mollier** and **SAM_Psychrometrics** should be used instead.

---

## Features

- Thermodynamic properties of water and steam based on **IAPWS-IF97**
- Support for multiple IF97 regions (liquid, vapor, saturation, supercritical)
- Consistent integration with SAM analytical workflows
- Reference-grade formulations suitable for engineering analysis

---

## Resources
- 📘 **SAM IAPWS Wiki:** https://github.com/SAM-BIM/SAM_IAPWS/wiki  
- 📘 **SAM Wiki:** https://github.com/SAM-BIM/SAM/wiki  
- 🧠 **SAM Core:** https://github.com/SAM-BIM/SAM  
- 🔬 **IAPWS:** https://www.iapws.org/  

---

## Installing

To install **SAM** using the Windows installer, download and run the  
[latest installer](https://github.com/SAM-BIM/SAM_Deploy/releases/latest).

Alternatively, you can build the toolkit from source using Visual Studio.  
See the main repository for details:  
👉 https://github.com/SAM-BIM/SAM

---

## Development notes

- Target framework: **.NET / C#**
- Thermodynamic formulations follow the IAPWS-IF97 specification
- Intended for pure water and steam (not humid air)
- New or modified `.cs` files must include the SPDX header from `COPYRIGHT_HEADER.txt`

---

## Licence

This repository is free software licensed under the  
**GNU Lesser General Public License v3.0 or later (LGPL-3.0-or-later)**.

Each contributor retains copyright to their respective contributions.  
The project history (Git) records authorship and provenance of all changes.

See:
- `LICENSE`
- `NOTICE`
- `COPYRIGHT_HEADER.txt`
