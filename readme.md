# Time Onion

This app is a smart **todo list** allowing to organize your tasks over a **time horizon** concept and **benefits**, instead of date and priorities.

## Domain and features

### Time horizons

Assigning a date to an item to do is not the optimal way to organize your items.
Instead it is preferable to assign a time horizon. It is a relative time period from now, always in the future.
- This day
- This week
- This month
- This quarter
- This year
- This life

The more the horizon is large, the bigger and blurier is the task to achieve.
The closer we get from this day, the smaller and detailed the tasks are.

### Assistance on time horizon switch

As the time flows, items must be progressively rescheduled from larger time horizon to smaller one (closer to today).
TimeOnion helps you to select and reorganize easily your tasks every begin of time horizon (ie: every day, take tasks from this week you want to achieve today. Every quarter begin, take tasks from this year you want to achieve during this quarter).

## Tech

The app is built with [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) hosted by server.
