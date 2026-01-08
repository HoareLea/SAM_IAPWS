[![Build (Windows)](https://github.com/SAM-BIM/SAM_IAPWS/actions/workflows/build.yml/badge.svg?branch=master)](https://github.com/SAM-BIM/SAM_IAPWS/actions/workflows/build.yml)
[![Installer (latest)](https://img.shields.io/github/v/release/SAM-BIM/SAM_Deploy?label=installer)](https://github.com/SAM-BIM/SAM_Deploy/releases/latest)


# SAM_IAPWS

<a href="https://github.com/HoareLea/SAM_IAPWS"><img src="https://github.com/HoareLea/SAM/blob/master/Grasshopper/SAM.Core.Grasshopper/Resources/SAM_Small.png" align="left" hspace="10" vspace="6"></a>

**SAM_IAPWS** is part of SAM Toolkit that is designed to help engineers calculate Water and Steam properties. Welcome and let's make the opensource journey continue. :handshake:

**IAPWS-IF97** is the **Industrial Formulation 1997** created by the **International Association for the Properties of Water and Steam**.
It is:

* 🔬 Designed **specifically for pure water and steam**
* 💧 Includes **liquid, vapor, and supercritical states** of water
* 🧪 Used extensively in:

  * Power plants (Rankine cycles)
  * Steam turbines
  * Boilers
  * Scientific computation involving water as a pure substance

❌ It **does not** cover:

* Dry air
* Mixtures like **humid air**
* Psychrometric properties (which require additional modeling like Dalton’s law and mixture rules)

---

## 📊 2. **IAPWS-IF97 Regions and Examples**

Here’s a breakdown of each region with real-world examples:

| Region | State                           | Description                                                         | Example                                                       |
| ------ | ------------------------------- | ------------------------------------------------------------------- | ------------------------------------------------------------- |
| **1**  | **Compressed/Subcooled Liquid** | Water below saturation temperature for given pressure               | Hot water in a pressurized heating loop (e.g. 120°C at 2 bar) |
| **2**  | **Superheated Vapor**           | Steam above saturation temperature at given pressure                | Turbine inlet steam (e.g. 300°C at 1 MPa)                     |
| **3**  | **Dense fluid (near critical)** | High-pressure region near water’s critical point                    | Water at 650 K and 22 MPa in supercritical boilers            |
| **4**  | **Saturation (phase boundary)** | Coexistence of saturated liquid and vapor — boiling/condensing line | Boiling water at 100°C and 1 atm                              |
| **5**  | **High-temp vapor**             | Very high temperature/low pressure steam (used in gas turbines)     | Steam at 800 K and 0.5 MPa                                    |

---

## 💡 3. **So What Do You Use for Humid Air?**

For **air–water vapor mixtures**, like in psychrometrics:

| Property             | Source                                                                                  |
| -------------------- | --------------------------------------------------------------------------------------- |
| Dry air              | Ideal gas model (often with constant $c_p$)                                             |
| Water vapor          | IAPWS-IF97 or simplified correlations (e.g. Tetens, ASHRAE)                             |
| Mixtures (humid air) | Use **psychrometric models**, combining both gases via Dalton’s Law and energy balances |

So, for psychrometrics:

* Use **IAPWS or SAM_Mollier** to calculate:

  * $p_s$ (saturation pressure)
  * $x$ (humidity ratio)
  * $h$ (enthalpy)
* Then mix with dry air properties accordingly

---

## 🧪 Example Use Cases: Side-by-Side

| Application              | Use IAPWS?       | Use Psychrometrics SAM_Mollier?                              |
| ------------------------ | ---------------- | ------------------------------------------------ |
| Steam turbine simulation | ✅ Yes            | ❌ No                                             |
| HVAC room air modeling   | ❌ No             | ✅ Yes                                            |
| Condensation in a boiler | ✅ Yes (Region 4) | ❌ Not needed                                     |
| Moist air inside AHU     | ❌ No             | ✅ Yes                                            |
| Fog/Ice fog detection    | ❌ No             | ✅ Yes — via humidity ratio & psychrometric logic |

---

## ✅ Summary

* **IAPWS-IF97** is for **pure water/steam** only.
* **Psychrometrics SAM_Mollier** is for **humid air** (water vapor + dry air).
* Both use overlapping physics (e.g. saturation pressure), but serve different systems.



## Resources
* [Wiki](https://github.com/SAM-BIM/SAM_IAPWS/wiki)

## Installing

To install **SAM** from .exe just download and run [latest installer](https://github.com/SAM-BIM/SAM_Deploy/releases) 

## Licence ##

SAM is free software licenced under GNU Lesser General Public Licence - [https://www.gnu.org/licenses/lgpl-3.0.html](https://www.gnu.org/licenses/lgpl-3.0.html)  
Each contributor holds copyright over their respective contributions.
The project versioning (Git) records all such contribution source information.
See [LICENSE](https://github.com/HoareLea/SAM_Template/blob/master/LICENSE) and [COPYRIGHT_HEADER](https://github.com/HoareLea/SAM/blob/master/COPYRIGHT_HEADER.txt).
