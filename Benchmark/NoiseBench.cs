using BenchmarkDotNet.Attributes;
using Noise;
using WhatIsAVector.Noise;

namespace Benchmark;

[MemoryDiagnoser]
public class NoiseBench
{
    private readonly Perlin _noise = new ();
    private readonly OpenSimplex2S _openSimplex2S = new(777);
    private readonly OpenSimplex2F _openSimplex2F = new(777);
    private float _offsetX = 0.1f;
    private float _offsetY = 0.1f;
    private float _offsetZ = 0.1f;

    [Benchmark]
    public void OpenSimplex2F2D()
    {
        var x = _openSimplex2F.Noise2(_offsetX, _offsetY);
        _offsetX += 0.01f;
        _offsetY += 0.01f;
    }
    
    [Benchmark]
    public void OpenSimplex2F3D()
    {
        var x = _openSimplex2F.Noise3_Classic(_offsetX, _offsetY, _offsetZ);
        _offsetX += 0.01f;
        _offsetY += 0.01f;
        _offsetZ += 0.01f;
    }    
    
    [Benchmark]
    public void OpenSimplex2S2D()
    {
        var x = _openSimplex2S.Noise2(_offsetX, _offsetY);
        _offsetX += 0.01f;
        _offsetY += 0.01f;
    }
    
    [Benchmark]
    public void OpenSimplex2S3D()
    {
        var x = _openSimplex2S.Noise3_Classic(_offsetX, _offsetY, _offsetZ);
        _offsetX += 0.01f;
        _offsetY += 0.01f;
        _offsetZ += 0.01f;
    }    

    [Benchmark]
    public void PerlinNoise1D()
    {
        var x = _noise.Noise(_offsetX);
        _offsetX += 0.01f;
    }
    
    [Benchmark]
    public void PerlinNoise2D()
    {
        var x = _noise.Noise(_offsetX, _offsetY);
        _offsetX += 0.01f;
        _offsetY += 0.01f;
    }
    
    [Benchmark]
    public void PerlinNoise3D()
    {
        var x = _noise.Noise(_offsetX, _offsetY, _offsetZ);
        _offsetX += 0.01f;
        _offsetY += 0.01f;
        _offsetZ += 0.01f;
    }
}
