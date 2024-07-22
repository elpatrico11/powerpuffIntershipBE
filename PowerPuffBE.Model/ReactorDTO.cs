﻿namespace PowerPuffBE.Model;

using Enums;

public class ReactorDTO
{
    public Guid Id {get; set;}
    public string Name {get; set;}
    public string Status {get; set;}
    public string Description {get; set;}
    
    public string ImageContent { get; set; }
    
    public IEnumerable<ReactorChartDTO> Reactorpowerproduction {get; set;}
    public IEnumerable<ReactorChartDTO> Reactorcoretemperature {get; set;}
    
    public List<LinkModelDTO> Links {get; set;}
}