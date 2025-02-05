using my_virtual_pets_api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using ImageRecognition;
using Castle.Core.Configuration;

namespace my_virtual_pets.Tests
{
    public class DragonEyeAPITests
    {
        public Mock<IRecognitionService> _mockRecognitionService { get; set; }
        public RecognitionService _recognitionService {  get; set; }

        private string testJson;
        private IPredicted testPredicted;

        [SetUp]
        public void Setup()
        {
            _mockRecognitionService = new Mock<IRecognitionService>();
            _recognitionService = new RecognitionService(); 

        }

        [Test]
        public void DeserializerReturnsObjectForValidPrediction()
        {
            testJson = """
                                {
                    "predictions": [
                        {
                            "category": {
                                "id": 713730275076072,
                                "type": "category",
                                "name": "mammals",
                                "displayName": "Mammals",
                                "score": null,
                                "uncalibrated_score": -1.0,
                                "children": [
                                    {
                                        "id": 713730275076072,
                                        "type": "category",
                                        "name": "mammals",
                                        "displayName": "Carnivores",
                                        "score": null,
                                        "uncalibrated_score": -1.0,
                                        "children": [
                                            {
                                                "id": 717939453480153,
                                                "type": "category",
                                                "name": "cat",
                                                "displayName": "Cat",
                                                "score": 1.0,
                                                "uncalibrated_score": 0.1185302734375,
                                                "children": []
                                            }
                                        ]
                                    }
                                ]
                            },
                            "traits": [],
                            "normalizedBbox": [
                                0.0517578125,
                                0.06734816596512327,
                                0.8818359375,
                                0.9482862297053518
                            ]
                        }
                    ]
                }
                """;
            testPredicted = new Child { id = 717939453480153, type = "category", name = "cat", displayName = "Cat", score = 1.0, uncalibrated_score = 0.1185302734375, children = [] };

            Assert.That(_recognitionService.Deserialize(testJson).Result, Is.TypeOf<Child>());
        }
        [Test]
        public void DeserializerReturnsNullForInvalidPrediction()
        {
            testJson = """
                               {
                    "predictions": [
                        {
                            "category": {
                                "id": 713730275076072,
                                "type": "category",
                                "name": "mammals",
                                "displayName": "Mammals",
                                "score": null,
                                "uncalibrated_score": -1.0,
                                "children": [
                                    {
                                        "id": 713730275076072,
                                        "type": "category",
                                        "name": "mammals",
                                        "displayName": "Carnivores",
                                        "score": null,
                                        "uncalibrated_score": -1.0,
                                        "children": [
                                            {
                                                "id": 717939453480153,
                                                "type": "category",
                                                "name": "cat",
                                                "displayName": "Cat",
                                                "score": 1.0,
                                                "uncalibrated_score": 0.12481689453125,
                                                "children": []
                                            }
                                        ]
                                    }
                                ]
                            },
                            "traits": [],
                            "normalizedBbox": [
                                0.7340286831812256,
                                0.34375,
                                0.9328552803129074,
                                0.83203125
                            ]
                        },
                        {
                            "category": {
                                "id": 713730275076072,
                                "type": "category",
                                "name": "mammals",
                                "displayName": "Mammals",
                                "score": null,
                                "uncalibrated_score": -1.0,
                                "children": [
                                    {
                                        "id": 713730275076072,
                                        "type": "category",
                                        "name": "mammals",
                                        "displayName": "Carnivores",
                                        "score": null,
                                        "uncalibrated_score": -1.0,
                                        "children": [
                                            {
                                                "id": 717939453480153,
                                                "type": "category",
                                                "name": "cat",
                                                "displayName": "Cat",
                                                "score": 0.6627485752105713,
                                                "uncalibrated_score": 0.10540771484375,
                                                "children": []
                                            }
                                        ]
                                    }
                                ]
                            },
                            "traits": [],
                            "normalizedBbox": [
                                0.27835723598435463,
                                0.421875,
                                0.5052151238591917,
                                0.8203125
                            ]
                        },
                        {
                            "category": {
                                "id": 713730275076072,
                                "type": "category",
                                "name": "mammals",
                                "displayName": "Mammals",
                                "score": null,
                                "uncalibrated_score": -1.0,
                                "children": [
                                    {
                                        "id": 713730275076072,
                                        "type": "category",
                                        "name": "mammals",
                                        "displayName": "Carnivores",
                                        "score": null,
                                        "uncalibrated_score": -1.0,
                                        "children": [
                                            {
                                                "id": 717939453480153,
                                                "type": "category",
                                                "name": "cat",
                                                "displayName": "Cat",
                                                "score": 0.983405590057373,
                                                "uncalibrated_score": 0.1256103515625,
                                                "children": []
                                            }
                                        ]
                                    }
                                ]
                            },
                            "traits": [],
                            "normalizedBbox": [
                                0.4765319426336376,
                                0.5419921875,
                                0.6981747066492829,
                                0.81640625
                            ]
                        }
                    ]
                }
                """;

            Assert.Null(_recognitionService.Deserialize(testJson).Result);
        }
        [Test]
        public void ValidDeserializationContainsAnimal()
        {
            testJson = """
                                {
                    "predictions": [
                        {
                            "category": {
                                "id": 713730275076072,
                                "type": "category",
                                "name": "mammals",
                                "displayName": "Mammals",
                                "score": null,
                                "uncalibrated_score": -1.0,
                                "children": [
                                    {
                                        "id": 713730275076072,
                                        "type": "category",
                                        "name": "mammals",
                                        "displayName": "Carnivores",
                                        "score": null,
                                        "uncalibrated_score": -1.0,
                                        "children": [
                                            {
                                                "id": 717939453480153,
                                                "type": "category",
                                                "name": "cat",
                                                "displayName": "Cat",
                                                "score": 1.0,
                                                "uncalibrated_score": 0.1185302734375,
                                                "children": []
                                            }
                                        ]
                                    }
                                ]
                            },
                            "traits": [],
                            "normalizedBbox": [
                                0.0517578125,
                                0.06734816596512327,
                                0.8818359375,
                                0.9482862297053518
                            ]
                        }
                    ]
                }
                """;
            testPredicted = new Child { id = 717939453480153, type = "category", name = "cat", displayName = "Cat", score = 1.0, uncalibrated_score = 0.1185302734375, children = [] };

            Assert.AreEqual(_recognitionService.Deserialize(testJson).Result.name, testPredicted.name);
        }
        [Test]
        public void ValidAnimalReturnsTrue()
        {
            testPredicted = new Child { id = 717939453480153, type = "category", name = "cat", displayName = "Cat", score = 1.0, uncalibrated_score = 0.1185302734375, children = [] };

            Assert.That(_recognitionService.CheckIfAnimal(testPredicted));
        }
        [Test]
        public void InvalidAnimalReturnsFalse()
        {
            testPredicted = new Child { id = 717939453480153, type = "category", name = "chair", displayName = "Chair", score = 1.0, uncalibrated_score = 0.1185302734375, children = [] };

            Assert.That(!_recognitionService.CheckIfAnimal(testPredicted));
        }
        [Test]
        public void ValidURLReturnsTrue()
        {
            string testURL = "http://google.com";

            Assert.That(_recognitionService.CheckIfUrl(testURL));
        }
        [Test]
        public void InvalidURLReturnsFalse()
        {
            string testURL = "this isn't a valid URL";

            Assert.That(!_recognitionService.CheckIfUrl(testURL));
        }
    }
}