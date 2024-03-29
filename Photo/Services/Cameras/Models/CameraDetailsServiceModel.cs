﻿using Photo.Model;

namespace Photo.Services.Cameras.Models;

public class CameraDetailsServiceModel
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string ModelCamera { get; set; }

    public decimal Price { get; set; }

    public List<Image> Imgs { get; set; }

    public string Description { get; set; }

    public int DealerId { get; set; }

    public string DealerName { get; set; }


}