import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GoalserviceService {
  private goalApiUrl = 'http://localhost:5010/api/GoalPlan/Retrieve';
  private goalApiUrl1 = 'http://localhost:5010/api/GoalPlan/Store';
  private goalApiUrl2='http://localhost:5010/api/MonthInvestment/Store';
  private apiUrl='http://localhost:5010/Savings'

  constructor(private http: HttpClient, private router: Router) {}

  getGoals(pId: number) {
    return this.http.get(`${this.goalApiUrl}/${pId}`);
  }

  addGoal(goalData: any) {
    return this.http.post(this.goalApiUrl1, goalData);
  }

  contribute(contributionData: any) {
    console.log("Sending API Request:", contributionData);
    return this.http.post(this.goalApiUrl2, contributionData);
  }

  // ✅ Fetch User Savings (Progress Data)
  getSavings(gId: number)
  { 
    return this.http.get<number>(`${this.apiUrl}/${gId}`);
  }
}
