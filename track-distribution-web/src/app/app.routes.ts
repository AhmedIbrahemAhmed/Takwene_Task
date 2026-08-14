import { Routes } from '@angular/router';
import { TrackList } from './track-list/track-list';
import { TrackDetail } from './track-detail/track-detail';

export const routes: Routes = [
    {path: '', redirectTo: '/tracks', pathMatch: 'full'},
    {path: 'tracks', component: TrackList},
    {path: 'tracks/:id', component: TrackDetail},
];
