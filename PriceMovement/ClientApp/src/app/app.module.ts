import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ButtonsModule } from '@progress/kendo-angular-buttons';
import { GridModule, ExcelModule } from '@progress/kendo-angular-grid';
import { InputsModule } from '@progress/kendo-angular-inputs';
import { DateInputsModule } from '@progress/kendo-angular-dateinputs';

import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';

import { AppComponent } from './app.component';
import { HttpErrorInterceptor } from './errorHandler';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './price-movement/price-movement.component';
import { SecurityDataComponent } from './price-movement/security-portfolio-movement.component';
import { FlatDataComponent } from './price-movement/flat-movement.component';
import { YieldPointComponent } from './price-movement/yield-point.component';
import { StaleYieldComponent } from './price-movement/stale-yield.component';
import { UnderlyingComponent } from './price-movement/underlying.component';
import { PriceDetailsComponent } from './price-movement/price-details.component';
import { PortfolioComponent } from './price-movement/portfolio-details.component';
import { PriceMovementService } from './services/price-movement.service';
import { SettingsComponent } from './settings/settings.component';
import { DataService } from './services/data.service';
import { StatePersistingService } from './services/state-persistence.service';

const appRoutes: Routes = [

  { path: '', component: HomeComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'fetch-data/:id', component: FetchDataComponent },
  { path: 'yield-point', component: YieldPointComponent },
  { path: 'stale-yield', component: StaleYieldComponent },
  { path: 'underlying', component: UnderlyingComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'home', component: HomeComponent, data: { title: 'Price Movement' } },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent,
    FlatDataComponent,
    SecurityDataComponent,
    PortfolioComponent,
    PriceDetailsComponent,
    YieldPointComponent,
    StaleYieldComponent,
    UnderlyingComponent,
    SettingsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    GridModule,
    BrowserAnimationsModule,
    ExcelModule ,
    ButtonsModule,
    MultiselectDropdownModule,
    InputsModule,
    DateInputsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true,
    },
    DataService,
    PriceMovementService,
    StatePersistingService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
