

## Elicitation

### Questions to Answer:

1. **Is the goal or outcome well defined?**
   - The goal is to create a wildfire tracking app with real-time updates, resources, and accessibility features.

2. **What is not clear?**
   - How will push notifications work? Are they location-based, subscription-based, or both?
   - Which weather API will be used, and what data will it provide (e.g., temperature, humidity, wind direction)?

3. **How about scope?**
   - The scope includes fire tracking, user notifications, and accessibility, but it’s unclear if multi-language options are included.
   - Does the project focus only on tracking and resources or extend to user-driven reporting systems?

4. **What do you not understand?**
   - **Technical Domain Knowledge:** Are there constraints for integrating mapping APIs (e.g., Leaflet.js with NASA FIRMS)?
   - How should we gather evacuation locations? Does NASA FIRMS have an API for this?

5. **Is there something missing?**
   - A clear user flow for how users subscribe to or interact with specific fires.
   - Details on database structure (e.g., storing fire progression, user data).
   - Accessibility testing methods.

6. **Answers to Gather:**
   - What are the primary technical challenges in pulling real-time data from multiple APIs?
   - Should fire tracking prioritize containment status, air quality, or progression over time?

---

## Analysis

### Steps for Analysis:

1. **Attributes, Terms, Entities, and Constraints:**
   - **Entities:**
     - Fire, User, Notification, Weather Data.
   - **Attributes:**
     - Fire: `ID`, `Location (latitude/longitude)`, `Containment Percentage`, (figure out where to get this data),`Progression (time-series data)`·(NASA FIRMS has this data).
     - User: `ID`, `Username, User-Location`, `Fire-Subscriptions, NotificationsEnabled`, `Notification Preferences (can be none)`.
     - Weather Data: `ID`, `Temperature`, `Wind Speed/Direction`, `Air Quality Index`.
   - **Constraints:**
     - Real-time API calls for fire and weather data must not exceed API limits.
     - Map layers must render without significant lag for multiple users.

2. **Requirement Conflicts or Missing Information:**
   - **Conflict:** Should fires be user-reported, API-driven, or a combination of both?
   - **Missing:** How will time-series data be visualized (graph, map animation)? NASA FIRMS has a timeline feature

3. **Actions:**
   - Return to elicitation for clarification on user flow and push notification triggers.

---

## Design and Modeling

### Initial Data Model:

1. **Entities and Attributes:**
   - **Fire:** `FireID`,  `Location`, `ContainmentStatus`, `Progression`, `StartDate`, 
   - **User:** `UserID`, `Username`, `Location`, `NotificationPreferences`, `NotificationsEnabled`.
   - **WeatherData:** `WeatherID`, `Temperature`, `WindSpeed`, `AirQualityIndex`.
   - **Notification:** `NotificationID`, `UserID`, `FireID`, `Message`, `Timestamp`.

2. **Relationships:**
   - A **Fire** can have multiple **WeatherData** entries (time-series updates).
   - A **User** can subscribe to multiple **Fires**.
   - A **Notification** is sent to a **User** regarding a **Fire**.

3. **English Descriptions:**
   - Users can subscribe to these fires for real-time notifications.
   - Notifications are triggered based on user preferences or fire updates.

4. **Entity-Relation Diagram (ERD):**
   - Draw informal ERD showing entities like **Fire**, **User**,  **WeatherData**, and **Notification**, with labeled relationships.

---

## Analysis of the Design

### Steps to Validate the Design:

1. **Support for Requirements:**
   - **Requirement Example:**  
     - *"As a user, I want to track specific fires."*
       - Steps: User subscribes to a fire → Backend stores subscription → Notification triggered by fire update.
       - **Can be done?** Yes. **Easily?** Needs seamless integration of user preferences with fire data.
     - *"As a user, I want to see fire progression over the last 24 hours."*
       - Steps: Time-series data from the NASA FIRMS API is fetched → Data is displayed via map animation.
       - **Can be done?** Yes, but requires careful rendering of time-series map layers.

2. **Non-Functional Requirements:**
   - **Performance:** Can the system render the maps with the fire, and weather information without lag?
   - **Usability:** Does the map meet accessibility standards (e.g., screen readers, intuitive interface)? Maybe location and its associated data can read out loud. Perhaps just make it accessible to the screen reader - in order to read the webpage out loud

---

## Next Steps:

1. **Return to Elicitation:**
   - Clarify user notification flow
   - Confirm scope for accessibility and multi-language support.

2. **Iterate on the Data Model:**
   - Refine relationships and attributes after gathering feedback.

3. **Prepare ERD and Validate Design:**
   - Create an informal ERD and review it with stakeholders.
   - Test if the design supports all critical user stories and non-functional requirements.