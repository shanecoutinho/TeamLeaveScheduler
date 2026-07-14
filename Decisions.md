# Design Decisions

## 1. 30% Team Capacity Rule

Decision:
The application uses `Math.Ceiling()` when calculating 30% of a team.

Reasoning:
For small teams, rounding down would make it impossible for anyone to take leave. Using ceiling ensures the rule remains practical while still limiting the number of employees on leave.

Alternative Considered:
Using `Math.Floor()` was considered but rejected because it is overly restrictive for small teams.

---

## 2. Multi-Day Leave Requests

Decision:
The 30% rule is checked for every working day in the requested leave period.

Reasoning:
A request may span several days, so availability should be validated daily rather than treating the entire period as a single block.

Alternative Considered:
Checking only the first day of the request was rejected because team capacity could be exceeded on later days.

---

## 3. Weekends and Public Holidays

Decision:
Weekends and public holidays are excluded from leave calculations.

Reasoning:
The challenge specifies that only working days count towards leave.

Alternative Considered:
Counting every calendar day was rejected because it would unfairly reduce employee leave balances.
