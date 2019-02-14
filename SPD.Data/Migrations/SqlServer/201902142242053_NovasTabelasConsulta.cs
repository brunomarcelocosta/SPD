namespace SPD.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NovasTabelasConsulta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SPD_CONSULTA",
                c => new
                    {
                        id_consulta = c.Int(nullable: false, identity: true),
                        fk_id_dentista = c.Int(nullable: false),
                        fk_id_pre_consulta = c.Int(nullable: false),
                        desc_procedimento = c.String(nullable: false, maxLength: 255),
                        odontograma = c.Binary(),
                        exame = c.Binary(),
                        comentarios = c.String(nullable: false, maxLength: 100),
                        dt_consulta = c.DateTime(nullable: false),
                        excluido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id_consulta, name: "SPD_CONSULTA_PK")
                .ForeignKey("dbo.SPD_DENTISTA", t => t.fk_id_dentista)
                .ForeignKey("dbo.SPD_PRE_CONSULTA", t => t.fk_id_pre_consulta)
                .Index(t => t.fk_id_dentista, name: "SPD_CONSULTA_SPD_DENTISTA_FK")
                .Index(t => t.fk_id_pre_consulta, name: "SPD_CONSULTA_SPD_PRE_CONSULTA_FK");
            
            CreateTable(
                "dbo.SPD_DENTISTA",
                c => new
                    {
                        id_dentista = c.Int(nullable: false, identity: true),
                        nome = c.String(nullable: false, maxLength: 50),
                        cro = c.String(nullable: false, maxLength: 50),
                        fk_id_usuario = c.Int(nullable: false),
                        dt_insert = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_dentista, name: "SPD_DENTISTA_PK")
                .ForeignKey("dbo.SPD_USUARIO", t => t.fk_id_usuario)
                .Index(t => t.fk_id_usuario, name: "SPD_DENTISTA_SPD_USUARIO_FK");
            
            CreateTable(
                "dbo.SPD_PRE_CONSULTA",
                c => new
                    {
                        id_pre_consulta = c.Int(nullable: false, identity: true),
                        fk_id_paciente = c.Int(nullable: false),
                        maior_idade = c.Boolean(nullable: false),
                        autorizado = c.Boolean(),
                        autorizacao = c.Binary(),
                        convenio = c.String(maxLength: 50),
                        nr_carterinha = c.String(maxLength: 50),
                        vl_consulta = c.String(maxLength: 15),
                        tp_pagamento = c.String(maxLength: 50),
                        dt_insert = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_pre_consulta, name: "SPD_PRE_CONSULTA_PK")
                .ForeignKey("dbo.SPD_PACIENTE", t => t.fk_id_paciente)
                .Index(t => t.fk_id_paciente, name: "SPD_CONSULTA_SPD_PACIENTE_FK");
            
            CreateTable(
                "dbo.SPD_HISTORICO_CONSULTA",
                c => new
                    {
                        id_historico_consulta = c.Int(nullable: false, identity: true),
                        fk_id_consulta = c.Int(nullable: false),
                        dt_consulta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_historico_consulta, name: "SPD_HISTORICO_CONSULTA_PK")
                .ForeignKey("dbo.SPD_CONSULTA", t => t.fk_id_consulta)
                .Index(t => t.fk_id_consulta, name: "SPD_HISTORICO_CONSULTA_SPD_CONSULTA_FK");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SPD_HISTORICO_CONSULTA", "fk_id_consulta", "dbo.SPD_CONSULTA");
            DropForeignKey("dbo.SPD_CONSULTA", "fk_id_pre_consulta", "dbo.SPD_PRE_CONSULTA");
            DropForeignKey("dbo.SPD_PRE_CONSULTA", "fk_id_paciente", "dbo.SPD_PACIENTE");
            DropForeignKey("dbo.SPD_CONSULTA", "fk_id_dentista", "dbo.SPD_DENTISTA");
            DropForeignKey("dbo.SPD_DENTISTA", "fk_id_usuario", "dbo.SPD_USUARIO");
            DropIndex("dbo.SPD_HISTORICO_CONSULTA", "SPD_HISTORICO_CONSULTA_SPD_CONSULTA_FK");
            DropIndex("dbo.SPD_PRE_CONSULTA", "SPD_CONSULTA_SPD_PACIENTE_FK");
            DropIndex("dbo.SPD_DENTISTA", "SPD_DENTISTA_SPD_USUARIO_FK");
            DropIndex("dbo.SPD_CONSULTA", "SPD_CONSULTA_SPD_PRE_CONSULTA_FK");
            DropIndex("dbo.SPD_CONSULTA", "SPD_CONSULTA_SPD_DENTISTA_FK");
            DropTable("dbo.SPD_HISTORICO_CONSULTA");
            DropTable("dbo.SPD_PRE_CONSULTA");
            DropTable("dbo.SPD_DENTISTA");
            DropTable("dbo.SPD_CONSULTA");
        }
    }
}
