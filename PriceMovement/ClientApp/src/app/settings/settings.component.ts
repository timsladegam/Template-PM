import { Component, OnInit, OnDestroy } from '@angular/core';

import { StatePersistingService } from '../services/state-persistence.service';
import { DataService } from '../services/data.service';

@Component({
  selector: 'app-home',
  templateUrl: './settings.component.html',
  styleUrls: ['../price-movement/price-movement.component.css']
})

export class SettingsComponent implements OnInit, OnDestroy {
  public loading = false;
  public today: Date = new Date();
  public autoCorrect = true;

  constructor(public dataService: DataService, public persistingService: StatePersistingService) {
  }

  // on initialization
  public ngOnInit(): void {
    // nothing
  }

  public ngOnDestroy(): void {
    console.log('save settings: ' + JSON.stringify(this.dataService.settings));
    this.persistingService.setDataSettings('dataSettings', this.dataService.settings);
  }
}
