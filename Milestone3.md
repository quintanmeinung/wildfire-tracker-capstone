# *Milestone 3*
### *Revised Vision Statement:*
- Our team seeks to create a website that would allow users to track active wildfires. It would allow users to search for their location and display all the wildfires within a certain area. Our aim is to help users who are being affected by these wildfires to be able to look up various resources such as evacuation locations, food pantries, evacuation guidelines, and air quality indicators. Our group chose this idea due to the recent wildfires in California that destroyed a large amount of the LA area. As climate change becomes more prevalent, we aim to bring awareness to this issue. There are currently a few websites that already exist such as [NIFC](https://www.nifc.gov/fire-information/maps), [AirNow](https://fire.airnow.gov/), [WatchDuty](https://app.watchduty.org/) . We believe our project is worth doing because it can be used to help saves lives, and two out of the three links don’t include resources like evacuation zones, or wind direction and pressure, which we hope to implement into our project as we believe it to be vital information to the user.
---
# *Personas for Wildfire Tracking App*
1. Resident of wildfire prone areas.
2. Park ranger/firefighter interested in tracking wildfire development.
4. Enviromental Journalist


### 1. Sarah Lopez 
**Age**: 35  
**Hometown**: Santa Rosa, CA  
**Occupation**: High School Teacher  
**Background**:  
  - Sarah lives with her two children and elderly mother in a wildfire-prone area. Over the years, she’s experienced multiple evacuation alerts and is concerned about being unprepared for future emergencies. She wants to be able to use an app that will allow her to see current wildifres in her area, so she can properly evacuate in time as her grandmother has mobility issues that could mean life or death in the event of a fire close to her home. As a Highschool teacher she would also like to provide a good wildfire app as a resource to her students to keep them aware and alert of any changes to the fires.

**Motivations**:  
  - Staying informed about nearby wildfires to ensure her family’s safety.  
  - Accessing reliable evacuation routes and shelter information.  
  - Knowing where to find resources like food, water, and medical supplies during emergencies.  
  - Provide her students with a useful resource to keep track of the fires.

**Pain Points**:  
  - Struggles to navigate multiple websites for real-time updates.  
  - Finds it difficult to track changes in wind direction and fire containment statuses.  
  - Worries about getting misinformation during crises.

**Goals**:  
  - Use the app for immediate alerts and detailed maps of fire zones.  
  - Easily locate nearby evacuation zones and resources.
  - Have an app that is easy to use and understand 
---
### 2. Jason Patel 
**Age**: 41  
**Hometown**: Flagstaff, AZ  
**Occupation**: Senior Park Ranger/ Volunteer Firefighter   
**Background**:  
  - Jason has been a park ranger for 15 years, managing large areas of forestland prone to wildfires.He also works as a volunteer firefighter as he is passionate of the foresland he manages and wants to protect the land and its wildlife as much as possible. He would like to have a reliable wildfire app that would be able to show nearby fires and the amount that has been burned in order to prepare the forestland he manages for a potential wildfire coming his way. He would also love to recommend a useful app to all the guests that visit the park as well as the other firefighters he works with, as real time updates are vital for such a job.

**Motivations**:  
  - Quickly assess wildfire spread and containment efforts to adjust on-ground strategies.  
  - Use accurate data about wind direction, pressure, and weather to inform decision-making.  
  - Identify nearby resources to aid firefighting efforts.
  - Protect forestland and its wildlife from potential fires.
  - Inform park guests of useful app during wildfire season. 

**Pain Points**:  
  - Lacks an intuitive, all-in-one platform for real-time wildfire updates.  
  - Needs seamless collaboration tools to share wildfire data with his team.  
  - Finds existing systems outdated or overly complicated.  

**Goals**:  
  - Utilize the app’s live fire tracking and containment percentage feature.  
  - Share wildfire updates with colleagues through the app.  

---
### 3. Enviromental Journalist
**Name**: Marcus Rivera  
**Age**: 30  
**Hometown**: San Francisco, CA  
**Occupation**: Freelance Environmental Journalist  
**Background**:  
  - Marcus writes for major news outlets about the effect of climate and its impact on wildfires, how they are causing them to be more frequent, even in their off season, and more deadly with the elements often increasing their destructive potential. He frequently travels to wildfire-affected areas to gather first hand stories and notes of the land after a wildfire has ravaged it. He would like to have an app that accuretly tracks wildfires so that he knows when a wildfire has died down and is safe to travel too. As well as the ability to determine the air quality of the area in order to inform his readers of potential health risks for them and himself.
**Motivations**:  
  - Access up-to-date wildfire maps to provide accurate reporting.  
  - Track resources and evacuation zones to inform the public.  
  - Use air quality data to report on public health concerns.  

**Pain Points**:  
  - Struggles with inconsistent wildfire data across sources.  
  - Finds it time-consuming to piece together fire updates, air quality, and evacuation information.  
  - Needs a simple way to verify data for reporting purposes.  

**Goals**:  
  - Use the app as a reliable source for wildfire and air quality updates.  
  - Share app visuals (e.g., fire maps) with readers to enhance articles.  

  ---
# *Timeline & Release Plan:*
#### *Milestone 3*
- Epic & bigger user stories
- Jira
- Requirements
- Stakeholder personas
- Architecture diagram

#### *Milestone 4*
- Product and Sprint Backlog finished
- Test all user stories in Sprint 1 to meet requirements
- Upload Initial data model appropriate for all user stories
#### *Winter 2025 Sprint 1*
- Basic MVC framework instantiated
- API keys secured
- Database schema
#### *Winter 2025 Sprint 2*
- Mapping integration
    - Leaflet.js
- Database & EF Core setup
- Sprint 1 Retrospective
#### *Winter 2025 Sprint 3*
- JIT Modeling and Design of product
- User accounts
- Produce Correct Map Layering Techniques
- Get started on Sprint 4 before the first day of spring term.
#### *Spring 2025 Sprint 1*
- Get started on home page layout/tabs
- User authentication
  - Security
  - Accessing user location
- User fire subscription
  - Users can track specific fires
#### *Spring 2025 Sprint 2*
- Implement notifications on changes to fires
  - Fire containment, growth, etc.
- History log of previous fires
- Add accessibility features
#### *Spring 2025 Sprint 3*
- Resource page
  - Fire safety information
  - Emergency hotlines
- Evacuation Zone page
- Integrate Weather API
- Add ability to see fire progression in last 24 hrs
#### *Spring 2025 Sprint 4*
- Clean up pages/ make them look presentable
- Add an “About page”
- Add language options
- Penetration Testing for potential bugs/missing features 
#### *Spring 2025 Sprint 5*
- Last minute bug fixes, documentation, maintenance.
- Practice AES presentation
#### *AES*
- Present
---

### *Revised Needs & Features:*
- **Needs:**
    - As a user I want to track specific fires.
    - As a user I want to receive updates when specific fires change.
    - As a user I want to be notified of fires in my area.
    - As a user I want to see the recent history of a fire.	
    - Map must be intuitive and interactive.
    - App must be usable by people with disabilities.
- **Features:**
    - Fire subscription
    - Push notifications
    - Access to user location
    - Accessibility options
        - Screen-reader compatibility
        - Color blindness compatibility
    - Time-series fire display
        - Fire progression over last 24hrs.

--- 
[***Revised Architecture Diagram***](https://miro.com/app/board/uXjVLtfw6hY=/?share_link_id=502328115308)

[***Jira***](https://architechs-capstone.atlassian.net/jira/software/projects/SCRUM/boards/1/backlog?epics=visible&issueParent=10008%2C10007%2C10003%2C10004%2C10005%2C10006%2C10009)



[***Requirements Elicitation Template***](https://docs.google.com/document/d/1G5bJHCLpJ1E75M5BHMVkOcxLyLK02R38Vl1VEplQ6-Q/edit?usp=sharing)

