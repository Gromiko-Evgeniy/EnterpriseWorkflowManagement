﻿using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageQueries;

public sealed record GetHiringStageByIdQuery(int Id) : IRequest<HiringStage> { }
