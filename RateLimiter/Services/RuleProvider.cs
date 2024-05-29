﻿using System;
using System.Collections.Generic;
using RateLimiter.Helpers;
using RateLimiter.Interfaces;

namespace RateLimiter.Services
{
    public class RuleProvider
    {
        public IDateTimeWrapper _dateTime { get; set; }
        private readonly Dictionary<string, Dictionary<string, List<IRule>>> _rules = new();
        private string _currentResource;
        private string _currentRegion;

        public RuleProvider(IDateTimeWrapper dateTimeWrapper)
            => _dateTime = dateTimeWrapper;


        public RuleProvider ConfigureResource(string resource)
        {
            _currentResource = resource;
            _rules[resource] = new Dictionary<string, List<IRule>>();
            return this;
        }

        public RuleProvider ForRegion(string region)
        {
            _currentRegion = region;
            if (!_rules[_currentResource].ContainsKey(region))
            {
                _rules[_currentResource][region] = new List<IRule>();
            }
            return this;
        }

        public RuleProvider AddRule(IRule rule)
        {
            _rules[_currentResource][_currentRegion].Add(rule);
            return this;
        }

        public Dictionary<string, List<IRule>> GetRulesForResource(string resource)
        {            
            if (_rules.TryGetValue(resource, out var regions))
            {
                return regions;
            }

            return new Dictionary<string, List<IRule>>();
        }
    }

}